namespace Events
{
    //Defines the different event types to be used in event data in enumeration form
    public enum EventType
    {
        CommandRoomChange,
        CommandAreaExit,
        ExecuteCombatRoom,

        ReceiveDebug,

        #region Factories

        CreateEnemy,
        OnEnemyDeath,
        CreatePickup,

        #endregion

    }
}
