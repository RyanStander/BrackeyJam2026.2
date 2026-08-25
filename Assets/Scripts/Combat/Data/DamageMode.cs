namespace Combat.Data
{
    public enum DamageMode
    {
        Normal, //cant hurt own faction
        IgnoreFaction, //hurts everyone, including self
        AllExceptSelf  //hurts everyone except self
    }
}
