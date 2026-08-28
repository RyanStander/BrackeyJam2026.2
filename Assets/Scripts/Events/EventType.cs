namespace Events
{
    //Defines the different event types to be used in event data in enumeration form
    public enum EventType
    {
        CommandRoomChange, // Archived

        ExecuteCombatRoom, // Archived

        #region Arena
        ReturnToHub,

        #region Waves
        WaveStart,
        WaveEnd,
        BossStart,
        BossEnd,
        WavesCompleted,
        #endregion
        #endregion

        #region Factories

        CreateEnemy,
        OnEnemyDeath,
        CreatePickup,

        #endregion

        #region Inventory

        UpdatePlayerScrapCount,
        UpdatePlayerHealth,

        #endregion
    }
}
