using System.Collections;
using System.Collections.Generic;
using Combat.Interfaces.Attack_Behaviours;
using Controllers;
using UnityEngine;

public class BossPunch : BossController, IAttackBehaviour
{
    [SerializeField] public float Cooldown => 2f;
    [SerializeField] public bool isBusy = false;
    [SerializeField] private float attackTimer;
    [SerializeField] private float punchDamage = 10f;
    [SerializeField] private float punchRange = 5f;
    [SerializeField] private float attackCooldown = 1f;
    private float dist;

    public bool CanExecute(AIController controller)
    {
        if (controller is BossController boss && boss.Phase > 0)
            return false;

        dist = Vector3.Distance(controller.transform.position, controller.Target.transform.position);
        return dist < 4f;
    }

    public void Telegraph(AIController controller) => controller.Animator.SetTrigger("Punch");
    public void Execute(AIController controller) 
    { 
        Debug.Log("Executing Punch!");
        isBusy = true;
        Animator.SetBool("isBusy", true);
        HitTarget(punchDamage, punchRange);
        attackTimer = attackCooldown;
        Target.GetComponent<Rigidbody>().AddForce(new Vector3(this.transform.position.x + this.transform.position.x, this.transform.position.z + this.transform.position.z), ForceMode.VelocityChange);
        Animator.SetBool("isBusy", false); //workaround for falling back to idle? 
        isBusy = false;
        //doing double dmg right now i dont know why
    }
    public bool IsFinished(AIController c) => true;
}