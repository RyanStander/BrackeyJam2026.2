using FMODUnity;

namespace AudioManagement
{
    public static class AudioDataHandler
    {
        private static SoundLibrary soundLibrary => AudioManager.SoundLibrary;

        public static class Music
        {
            public static EventReference HUB => soundLibrary.MusicSoundLibrary.HUB;
            public static EventReference BabyLevel => soundLibrary.MusicSoundLibrary.BabyLevel;
            public static EventReference NormalLevel => soundLibrary.MusicSoundLibrary.NormalLevel;
            public static EventReference BossLevel => soundLibrary.MusicSoundLibrary.BossLevel;
        }
        
        public static class Player
        {
            public static EventReference AttackHit => soundLibrary.PlayerSoundLibrary.AttackHit;
            public static EventReference AttackSwing => soundLibrary.PlayerSoundLibrary.AttackSwing;
            public static EventReference Dodge => soundLibrary.PlayerSoundLibrary.Dodge;
            public static EventReference Running => soundLibrary.PlayerSoundLibrary.Running;
            public static EventReference Walking => soundLibrary.PlayerSoundLibrary.Walking;
        }

        public static class Gunslinger
        {
            public static EventReference BasicShot => soundLibrary.GunslingerSoundLibrary.BasicShot;
            public static EventReference Reload => soundLibrary.GunslingerSoundLibrary.Reload;
            public static EventReference SpecialShot => soundLibrary.GunslingerSoundLibrary.SpecialShot;
            public static EventReference Walking => soundLibrary.GunslingerSoundLibrary.Walking;
        }

        public static class Boss
        {
            public static EventReference BossFlight => soundLibrary.BossSoundLibrary.BossFlight;
            public static EventReference ChargeFlight => soundLibrary.BossSoundLibrary.ChargeFlight;
            public static EventReference FlightPunchCombo => soundLibrary.BossSoundLibrary.FlightPunchCombo;
            public static EventReference Punch => soundLibrary.BossSoundLibrary.Punch;
            public static EventReference Slam => soundLibrary.BossSoundLibrary.Slam;
        }
        
        public static class Swatter
        {
            public static EventReference Attack => soundLibrary.SwatterSoundLibrary.Attack;
            public static EventReference TakeDamage => soundLibrary.SwatterSoundLibrary.TakeDamage;
            public static EventReference Walking => soundLibrary.SwatterSoundLibrary.Walking;
        }

        public static class Charger
        {
            public static EventReference Attack => soundLibrary.ChargerSoundLibrary.Attack;
            public static EventReference Exposed => soundLibrary.ChargerSoundLibrary.Exposed;
            public static EventReference Walking => soundLibrary.ChargerSoundLibrary.Walking;
        }
    }
}
