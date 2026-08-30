using System;
using AudioManagement;
using Combat.Interfaces.Attack_Behaviours.AttackExtensions;
using Combat.Interfaces.Attack_Behaviours.Configs;
using Controllers;
using FMODUnity;
using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours
{
    public class ShootAttack : MonoBehaviour, IAttackBehaviour
    {
        [SerializeField] private ShootAttackConfig config;
        [SerializeField] private Vector3 offset = new(0, 2, 0);

        private enum Phase
        {
            Windup,
            Shooting,
            Reload,
            Done,
        }

        private enum ShooterType
        {
            Gunslinger,
            Swatter
        }

        [SerializeField] private ShooterType shooterType;

        private Phase phase;
        private float timer;
        private Vector3 targetDirection;

        public bool CanExecute(AIController controller) =>
            controller.Target != null && Vector3.Distance(controller.transform.position,
                controller.Target.transform.position) <=
            config.AttackDistance;

        public void Telegraph(AIController controller)
        {
            phase = Phase.Windup;
            timer = 0f;
            controller.AnimationController.PauseOnCurrentFrame();
        }

        public void Execute(AIController controller)
        {
            timer += Time.deltaTime;

            if (phase == Phase.Windup && timer >= config.WindupTime)
            {
                if (controller.Target == null)
                {
                    phase = Phase.Done;
                    timer = 0f;
                    controller.AnimationController.PauseOnCurrentFrame();
                    return;
                }

                phase = Phase.Shooting;
                timer = 0f;
                targetDirection = (controller.Target.transform.position - controller.transform.position).normalized;
                controller.AnimationController.PlayShoot(targetDirection);
            }
            else if (phase == Phase.Shooting && timer >= config.ReloadTime)
            {
                if (controller.Target == null)
                {
                    phase = Phase.Done;
                    timer = 0f;
                    controller.AnimationController.PlayRun(targetDirection);
                    controller.AnimationController.PauseOnCurrentFrame();
                    return;
                }

                GameObject proj = Instantiate(config.ProjectilePrefab, controller.transform.position + offset,
                    Quaternion.identity);
                proj.GetComponent<Projectile>().Launch(controller.Target, config.Damage, controller,
                    config.ProjectileSpeed, controller.Faction);
                switch (shooterType)
                {
                    case ShooterType.Gunslinger:
                        AudioManager.PlayOneShot(AudioDataHandler.Gunslinger.BasicShot);
                        break;
                    case ShooterType.Swatter:
                        AudioManager.PlayOneShot(AudioDataHandler.Swatter.Attack);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                timer = 0f;

                phase = Phase.Reload;
                controller.AnimationController.PauseOnCurrentFrame();
            }
            else if (phase == Phase.Reload && timer >= config.ReloadTime)
            {
                phase = Phase.Done;
                timer = 0f;

                switch (shooterType)
                {
                    case ShooterType.Gunslinger:
                        AudioManager.PlayOneShot(AudioDataHandler.Gunslinger.Reload);
                        break;
                    case ShooterType.Swatter:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                controller.AnimationController.PlayRun(targetDirection);
                controller.AnimationController.PauseOnCurrentFrame();
            }
        }

        public bool IsFinished(AIController controller) => phase == Phase.Done;

        public float Cooldown => config.Cooldown;
    }
}
