using System.Numerics;
using Combat.Interfaces.Attack_Behaviours.Configs;
using Controllers;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Combat.Interfaces.Attack_Behaviours
{
    public class ChargeAttack : IAttackBehaviour
    {
        private ChargeAttackConfig config;
        
        public bool CanExecute(AIController owner)=>Vector3.Distance(owner.transform.position, owner.GetComponent<AIController>().Target.transform.position) <= config.AttackRange;

        public bool Telegraph(AIController owner)
        {
            throw new System.NotImplementedException();
        }

        public void Execute(AIController owner)
        {
            throw new System.NotImplementedException();
        }

        public bool IsFinished(AIController owner)
        {
            throw new System.NotImplementedException();
        }

        public float Cooldown { get; }
    }
}
