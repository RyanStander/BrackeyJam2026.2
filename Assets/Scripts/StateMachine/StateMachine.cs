using System;
using Combat.Interfaces.Attack_Behaviours;
using Controllers;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine
    {
        private IAttackBehaviour currentAttack;
        private AIController controller;
        private float stunTimer;
        
        public void Setup(AIController controller)
        {
            this.controller = controller;
        }
        
        enum State
        {
            Follow,//For Companion when no enemies
            Chase,
            Attacking,
            Recovering,
            Stunned
        }

        private State state = State.Chase;

        public void Tick()
        {
            switch (state)
            {
                case State.Follow:
                    
                    break;
                case State.Chase:
                    if (controller.Target==null || !controller.Target.activeInHierarchy)
                    {
                        controller.ReacquireTarget();
                        if (controller.Target == null)
                            break;
                    }
                    
                    controller.Movement.MovementTick(controller.Target.transform.position);
                    IAttackBehaviour attackCandidate = controller.PickAvailableAttack();
                    if (attackCandidate != null)
                    {
                        currentAttack = attackCandidate;
                        currentAttack.Telegraph(controller);
                        state = State.Attacking;
                    }
                    break;
                case State.Attacking:
                    State stateBeforeExecute = state;
                    currentAttack.Execute(controller);
    
                    if (state != stateBeforeExecute)
                        break;
    
                    if (currentAttack.IsFinished(controller))
                    {
                        controller.Movement.EndManualOverride();
                        state = State.Recovering;
                    }
                    break;
                case State.Recovering:
                    state = State.Chase;
                    break;
                case State.Stunned:
                    stunTimer -= Time.deltaTime;
                    if (stunTimer <= 0)
                    {
                        state = State.Chase;
                        controller.Animator.SetTrigger("End");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Stun(float duration)
        {
            state = State.Stunned;
            stunTimer = duration;
        }
    }
}
