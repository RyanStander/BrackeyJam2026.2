using System;
using Arena.Wave;
using AudioManagement;
using UnityEngine;

namespace Arena
{
    [CreateAssetMenu(fileName = "ArenaData", menuName = "Arena/ArenaData")]
    public class ArenaData : ScriptableObject
    {
        [field: SerializeField]
        public WaveDataSet Waves { get; private set; }

        [field: SerializeField, Range(0, 5)]
        public int WaveDelay { get; private set; } = 5;
        
        private enum SongToPlay
        {
            BabyLevel,
            NormalLevel,
            BossLevel,
        }
        [SerializeField]private SongToPlay chosenSong;

        public void PlaySong()
        {
            switch (chosenSong)
            {
                case SongToPlay.BabyLevel:
                    AudioManager.PlayMusic(AudioDataHandler.Music.BabyLevel);
                    break;
                case SongToPlay.NormalLevel:
                    AudioManager.PlayMusic(AudioDataHandler.Music.NormalLevel);
                    break;
                case SongToPlay.BossLevel:
                    AudioManager.PlayMusic(AudioDataHandler.Music.BossLevel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
