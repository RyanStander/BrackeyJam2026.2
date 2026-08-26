using Controllers;

namespace Combat.Interfaces.Attack_Behaviours
{
    public interface IAttackBehaviour
    {
        public bool CanExecute(AIController controller);
        public void Telegraph(AIController controller);
        public void Execute(AIController controller);
        public bool IsFinished(AIController controller);
        public float Cooldown { get;  }
    }
}
