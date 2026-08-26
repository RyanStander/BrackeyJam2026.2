using System;
using Combat.Interfaces.Attack_Behaviours.Configs;
using Controllers;
using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours
{
    public class ThrowAttack : MonoBehaviour, IAttackBehaviour
    {
        [SerializeField] private ThrowAttackConfig config;
        [SerializeField] private Throwable throwable;
        private Vector3 startLocation;
        private Vector3 targetLocation;

        private enum Phase
        {
            Windup,
            Throw,
            Return,
            Done,
        }

        private Phase phase;
        private float timer;

        private void OnValidate()
        {
            if (throwable == null)
                throwable = GetComponentInChildren<Throwable>();
        }

        public bool CanExecute(AIController controller) =>
            Vector3.Distance(controller.transform.position,
                controller.Target.transform.position) <=
            config.AttackDistance;

        public void Telegraph(AIController controller)
        {
            phase = Phase.Windup;
            timer = 0f;
            controller.Animator.SetTrigger("Windup");
        }

        public void Execute(AIController controller)
        {
            timer += Time.deltaTime;

            switch (phase)
            {
                case Phase.Windup when timer >= config.WindupTime:
                    phase = Phase.Throw;
                    timer = 0f;
                    controller.Animator.SetTrigger("Throw");
                    throwable.gameObject.SetActive(true);
                    startLocation = throwable.transform.position;
                    targetLocation = controller.Target.transform.position;
                    throwable.SetValues(config.Damage);
                    break;
                case Phase.Throw:
                {
                    throwable.Throw(targetLocation, config.ThrowSpeed);
                    
                    if(throwable.HitSomething())
                    {
                        if (config.ReturnWeapon)
                            phase = Phase.Return;
                        else
                        {
                            controller.Animator.SetTrigger("End");
                            phase = Phase.Done;
                        }
                    }

                    break;
                }
                case Phase.Return:
                {
                    throwable.Return(startLocation, config.ReturnSpeed);
                    if(throwable.Returned())
                    {
                        controller.Animator.SetTrigger("End");
                        throwable.gameObject.SetActive(false);
                        phase = Phase.Done;
                        throwable.Reset();
                    }
                    break;
                }
            }
        }

        public bool IsFinished(AIController controller) => phase == Phase.Done;

        public float Cooldown { get; }
    }
}
