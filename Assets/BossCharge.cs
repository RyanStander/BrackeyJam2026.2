using System.Collections;
using System.Collections.Generic;
using Combat.Interfaces.Attack_Behaviours;
using Controllers;
using UnityEngine;

public class BossCharge : BossController, IAttackBehaviour
{
    [SerializeField] public float Cooldown => 2f;
    [SerializeField] public bool isBusy = false;
    [SerializeField] private float attackTimer;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float chargeSpeed = 100f;
    [SerializeField] private float chargeDamage = 30f;
    private float dist;

    public bool CanExecute(AIController controller)
    {
        if (controller is BossController boss && boss.Phase > 0)
            return false;

        dist = Vector3.Distance(controller.transform.position, controller.Target.transform.position);
        return dist < 30f;
    }

    public void Telegraph(AIController controller) => controller.Animator.SetTrigger("Charge");
    public void Execute(AIController controller) 
    { 
        Debug.Log("Trying Charge!");
        isBusy = true;
        Animator.SetBool("isBusy", true);
            Vector3 dir = (Target.transform.position - transform.position).normalized;
            for (float t = 0; t < 5f; t += Time.deltaTime)
            {
                Movement.MovePosition(transform.position + dir * (chargeSpeed * Time.deltaTime));
                if (Vector3.Distance(transform.position, Target.transform.position) < 1.5f)
                {
                    Debug.Log("Hitting target!");
                    HitTarget(chargeDamage, 2f);
                    break;
                }
            }
            attackTimer = attackCooldown;
            isBusy = false;
            Animator.SetBool("isBusy", false);
    }
    public bool IsFinished(AIController c) => true;
}