using Combat.Data;
using Combat.Interfaces;

namespace Combat.Rules
{
    public class CombatRules
    {
        public static bool CanDamage(DamageInfo info, IDamageable target) {
            switch (info.Mode) {
                case DamageMode.IgnoreFaction:
                    return true;
                case DamageMode.AllExceptSelf:
                    return target.GameObject != info.Instigator;
                case DamageMode.Normal:
                default:
                    return info.SourceFaction != target.Faction;
            }
        }
    }
}
