using FMODUnity;

namespace AudioManagement
{
    public static class AudioDataHandler
    {
        private static SoundLibrary soundLibrary => AudioManager.SoundLibrary;

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

        public static class Charger
        {
            public static EventReference Attack => soundLibrary.ChargerSoundLibrary.Attack;
            public static EventReference Exposed => soundLibrary.ChargerSoundLibrary.Attack;
            public static EventReference Walking => soundLibrary.ChargerSoundLibrary.Walking;
        }
    }
}
