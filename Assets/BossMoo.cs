using System.Collections;
using UnityEngine;
using Combat.Data;
using Combat.Interfaces;
using Combat.Rules;

namespace Controllers
{
    public class BossMoo : AIController
    {
        [SerializeField] private float phase2Threshold = 15f;
        public int Phase { get; private set; } = 0;
        [SerializeField] private float attackCooldown = 2f;

        [Header("Damage / Range")]
        [SerializeField] private float punchDamage = 8f;
        [SerializeField] private float punchRange = 4f;
        [SerializeField] private float slapDamage = 15f;
        [SerializeField] private float slapRadius = 4f;
        [SerializeField] private float slamDamage = 10f;
        [SerializeField] private float slamRadius = 4f;
        [SerializeField] private float chargeSpeed = 15f;
        [SerializeField] private float chargeDamage = 20f;
        [SerializeField] private float grabDamage = 12f;
        [SerializeField] private float grabHeal = 10f;

        private float attackTimer;
        private bool isBusy;

        protected virtual void Update()
        {
            if (!Target.activeInHierarchy) 
            { 
                ReacquireTarget(); 
                return; 
            }
            if (isBusy) return;

            if (Phase == 0 && Health.CurrentHealth <= phase2Threshold)
            {
                Phase = 1;
                Debug.Log("Boss Moo has entered Phase 2!");
                //Animator.SetTrigger("GrowWings"); did not get this one done : ( 
            }

            Movement.MovementTick(Target.transform.position); 

            attackTimer -= Time.deltaTime;
            if (attackTimer > 0f) return;

            float distance = Vector3.Distance(transform.position, Target.transform.position);
            

            if (Phase == 0)
            {
                if (distance < punchRange) 
                {
                    Debug.Log("Boss Moo is performing a punch attack!");
                    StartCoroutine(DoPunch());
                }

                else if (distance < slapRadius)
                {
                   Debug.Log("Boss Moo is performing a ground slap attack!");
                   StartCoroutine(DoGroundSlap()); 
                } 
            }
            else
            {
                int roll = Random.Range(0, 3);
                if (roll == 0 && distance > 5f) 
                {
                    Debug.Log("Boss Moo is performing a charge attack!");
                    StartCoroutine(DoCharge());
                }
                else if (roll == 1)
                {
                    Debug.Log("Boss Moo is performing a multi-slam attack!");
                    StartCoroutine(DoMultiSlam());
                }
                else
                {
                    Debug.Log("Boss Moo is performing a grab attack!");
                    StartCoroutine(DoGrab());
                }
            }
        }

        private IEnumerator DoPunch()
        {
            isBusy = true;
            //Animator.SetTrigger("Punch"); or this one 
            yield return new WaitForSeconds(0.4f);
            HitTarget(punchDamage, punchRange);
            attackTimer = attackCooldown;
            isBusy = false;
        }

        private IEnumerator DoGroundSlap()
        {
            isBusy = true;
            //Animator.SetTrigger("Slap"); or this one 
            yield return new WaitForSeconds(0.6f);
            HitAoe(slapDamage, slapRadius);
            attackTimer = attackCooldown;
            isBusy = false;
        }

        private IEnumerator DoMultiSlam()
        {
            isBusy = true;
            //Animator.SetTrigger("Slam"); or this one 
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.4f);
                HitAoe(slamDamage, slamRadius);
            }
            attackTimer = attackCooldown;
            isBusy = false;
        }

        private IEnumerator DoCharge()
        {
            isBusy = true;
            //Animator.SetTrigger("Charge"); or this one 
            yield return new WaitForSeconds(0.8f);

            Vector3 dir = (Target.transform.position - transform.position).normalized;
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                Movement.MovePosition(transform.position + dir * (chargeSpeed * Time.deltaTime));
                if (Vector3.Distance(transform.position, Target.transform.position) < 1.5f)
                {
                    HitTarget(chargeDamage, 2f);
                    break;
                }
                yield return null;
            }
            attackTimer = attackCooldown;
            isBusy = false;
        }

        private IEnumerator DoGrab()
        {
            isBusy = true;
            Animator.SetTrigger("Grab");
            yield return new WaitForSeconds(0.3f);
            if (HitTarget(grabDamage, 2f))
                Health.CurrentHealth = Mathf.Min(Health.CurrentHealth + grabHeal, Health.MaxHealth);
            attackTimer = attackCooldown;
            isBusy = false;
        }

        private bool HitTarget(float dmg, float range)
        {
            if (Target == null) 
            {
                return false;
            }
            if (Vector3.Distance(transform.position, Target.transform.position) > range) 
            {
                return false;
            }
            var damageable = Target.GetComponent<IDamageable>();

            var info = new DamageInfo(dmg, Faction, gameObject);

            if (!CombatRules.CanDamage(info, damageable)) 
            {
                return false;
            }
            damageable.TakeDamage(info);
            return true;
        }

        private void HitAoe(float dmg, float radius)
        {
            foreach (var colliders in Physics.OverlapSphere(transform.position, radius))
            {
                var damageable = colliders.GetComponent<IDamageable>();

                if (damageable == null) continue;

                var info = new DamageInfo(dmg, Faction, gameObject);

                if (CombatRules.CanDamage(info, damageable)) 
                {
                    damageable.TakeDamage(info);
                }
            }
        }
    }
}