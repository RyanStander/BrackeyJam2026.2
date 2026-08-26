namespace Events
{
    //Event that informs subscribers of a debug log
    public class SendDebugLog : EventData
    {
        public readonly string Debuglog;

        public SendDebugLog(string givenLog) : base(EventType.ReceiveDebug)
        {
            Debuglog = givenLog;
        }
    }
    
    public class CreateEnemy : EventData
    {
        public readonly Factories.EnemyType EnemyType;
        public readonly UnityEngine.Vector3 Position;

        public CreateEnemy(Factories.EnemyType enemyType, UnityEngine.Vector3 position) : base(EventType.CreateEnemy)
        {
            EnemyType = enemyType;
            Position = position;
        }
    }
}
