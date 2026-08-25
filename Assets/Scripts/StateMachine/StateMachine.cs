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
        
        public void Setup(AIController controller)
        {
            this.controller = controller;
        }
        
        enum State
        {
            Chase,
            Attacking,
            Recovering,
            Staggered
        }

        private State state = State.Chase;

        public void Tick()
        {
            switch (state)
            {
                case State.Chase:
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
                    currentAttack.Execute(controller);
                    if (currentAttack.IsFinished(controller))
                    {
                        state = State.Recovering;
                    }
                    break;
                case State.Recovering:
                    state = State.Chase;
                    break;
                case State.Staggered:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
