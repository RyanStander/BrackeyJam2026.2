namespace Events
{
    //Defines the different event types to be used in event data in enumeration form
    public enum EventType
    {
        CommandRoomChange, // Archived
        CommandAreaExit, // Archived
        ExecuteCombatRoom, // Archived

        ReceiveDebug,

        #region Factories

        CreateEnemy,
        OnEnemyDeath,
        CreatePickup,

        #endregion

    }
}
