namespace Events
{
    //Defines the different event types to be used in event data in enumeration form
    public enum EventType
    {
        CommandRoomChange, // Archived
        CommandAreaExit, // Archived
        ExecuteCombatRoom, // Archived

        ReceiveDebug,

        #region Waves
        OnWaveStart,
        OnWaveEnd,
        OnBossStart,
        OnBossEnd,
        #endregion


        #region Factories

        CreateEnemy,
        OnEnemyDeath,
        CreatePickup,

        #endregion
    }
}