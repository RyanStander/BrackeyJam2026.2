using Controllers;

namespace Combat.Interfaces.Attack_Behaviours
{
    public interface IAttackBehaviour
    {
        public bool CanExecute(AIController owner);
        public bool Telegraph(AIController owner);
        public void Execute(AIController owner);
        public bool IsFinished(AIController owner);
        public float Cooldown { get;  }
    }
}
