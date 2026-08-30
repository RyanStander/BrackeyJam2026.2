using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace AudioManagement
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        private EventInstance _currentMusic;
        private EventInstance _currentAmbience;
        private EventReference _currentMusicReference;
        private EventReference _currentAmbienceReference;
        
        private Dictionary<int, EventInstance> _activeSounds = new();
        private int _nextId = 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        #region Static Wrappers

        /// <summary>
        /// Plays a one-shot sound effect. Use this for sounds that don't need to be stopped or have parameters changed after being played.
        /// </summary>
        /// <param name="sound">Use AudioDataHandler.[LIBRARY].soundName</param>
        public static void PlayOneShot(EventReference sound)
        {
            RuntimeManager.PlayOneShot(sound);
        }

        /// <summary>
        ///  Plays music. If the same music is already playing, it won't restart. If different music is playing, it will stop the current music and start the new one.
        /// </summary>
        /// <param name="music">Use AudioDataHandler.[LIBRARY].musicName</param>
        public static void PlayMusic(EventReference music)
        {
            if (Instance == null) return;
            Instance.PlayMusicInstance(music);
        }

        /// <summary>
        /// Stops the currently playing music. If fadeOut is true, the music will fade out instead of stopping immediately.
        /// </summary>
        /// <param name="fadeOut">Whether to allow the music to fade out or stop it immediately.</param>
        public static void StopMusic(bool fadeOut = true)
        {
            if (Instance == null) return;
            Instance.StopMusicInstance(fadeOut);
        }
        
        /// <summary>
        ///  Plays ambience. If the same ambience is already playing, it won't restart. If different ambience is playing, it will stop the current ambience and start the new one.
        /// </summary>
        /// <param name="ambience">Use AudioDataHandler.[LIBRARY].ambienceName</param>
        public static void PlayAmbience(EventReference ambience)
        {
            if (Instance == null) return;
            Instance.PlayAmbienceInstance(ambience);
        }
        
        /// <summary>
        ///  Stops the currently playing ambience. If fadeOut is true, the ambience will fade out instead of stopping immediately.
        /// </summary>
        /// <param name="fadeOut"> Whether to allow the ambience to fade out or stop it immediately.</param>
        public static void StopAmbience(bool fadeOut = true)
        {
            if (Instance == null) return;
            Instance.StopAmbienceInstance(fadeOut);
        }
        
        /// <summary>
        /// Plays a sound effect with optional parameters. Use this for sounds that need to have parameters set or changed after being played. Parameters should be passed as tuples of (parameterName, parameterValue).
        /// You can view parameters by opening FMOD top left where the File button is in unity editor->event browser->events and then you can navigate through there, if you click on sounds, some may have params at the bottom, you can change it to preview the effect.
        /// </summary>
        /// <param name="sound">Use AudioDataHandler.[LIBRARY].soundName</param>
        /// <param name="parameters">Tuples of (parameterName, parameterValue) to set on the sound instance. For example: ("Intensity", 0.5f), ("IsAlert", 1f)</param>
        public static void Play(EventReference sound, params (string name, float value)[] parameters)
        {
            if (Instance == null) return;
         
            Instance.PlayInstance(sound, parameters);
        }
        
        /// <summary>
        ///  Plays a looping sound effect and returns a handle to it.
        /// Use this for sounds that need to be stopped or have parameters changed after being played.
        /// Remember to stop the sound using the returned handle when it's no longer needed to free up resources.
        /// </summary>
        /// <param name="sound">Use AudioDataHandler.[LIBRARY].soundName</param>
        /// <returns>A SoundHandle that can be used to set parameters or stop the sound.</returns>
        public static SoundHandle PlayLoop(EventReference sound)
        {
            if (Instance == null) return default;
            return Instance.PlayLoopInstance(sound);
        }
        
        /// <summary>
        ///  Sets a parameter on a currently playing sound instance identified by the handle.
        /// This is used for sounds played with PlayLoop that need to have their parameters changed after being played.
        /// </summary>
        /// <param name="handle">The SoundHandle returned when the sound was played with PlayLoop.</param>
        /// <param name="param">The name of the parameter to set. This should match the parameter name defined in FMOD for that sound.</param>
        /// <param name="value">The value to set the parameter to. The valid range and meaning of this value depends on how the parameter is defined in FMOD.</param>
        public static void SetParameter(SoundHandle handle, string param, float value)
        {
            if (Instance == null) return;
            Instance.SetParameterInstance(handle, param, value);
        }
        
        /// <summary>
        ///  Stops a currently playing sound instance identified by the handle.
        /// If fadeOut is true, the sound will fade out instead of stopping immediately.
        /// </summary>
        /// <param name="handle">The SoundHandle returned when the sound was played with PlayLoop.</param>
        /// <param name="fadeOut">Whether to allow the sound to fade out or stop it immediately.</param>
        public static void Stop(SoundHandle handle, bool fadeOut = true)
        {
            if (Instance == null) return;
            Instance.StopInstance(handle, fadeOut);
        }

        #endregion

        #region Instance Methods
        
        private void PlayMusicInstance(EventReference music)
        {
            if (_currentMusicReference.Equals(music))
                return;

            StopMusicInstance();

            _currentMusic = RuntimeManager.CreateInstance(music);
            _currentMusic.start();

            _currentMusicReference = music;
        }

        private void StopMusicInstance(bool fadeOut = true)
        {
            if (!_currentMusic.isValid())
                return;

            _currentMusic.stop(
                fadeOut
                    ? STOP_MODE.ALLOWFADEOUT
                    : STOP_MODE.IMMEDIATE
            );

            _currentMusic.release();
        }
        
        private void PlayAmbienceInstance(EventReference ambience)
        {
            if (_currentAmbienceReference.Equals(ambience))
                return;

            StopAmbienceInstance();

            _currentAmbience = RuntimeManager.CreateInstance(ambience);
            _currentAmbience.start();

            _currentAmbienceReference = ambience;
        }

        private void StopAmbienceInstance(bool fadeOut = true)
        {
            if (!_currentAmbience.isValid())
                return;

            _currentAmbience.stop(
                fadeOut
                    ? STOP_MODE.ALLOWFADEOUT
                    : STOP_MODE.IMMEDIATE
            );

            _currentAmbience.release();
        }

        private void PlayInstance(EventReference sound, params (string name, float value)[] parameters)
        {
            EventInstance instance = RuntimeManager.CreateInstance(sound);

            foreach ((string name, float value) parameter in parameters)
            {
                instance.setParameterByName(parameter.name, parameter.value);
            }

            instance.start();
            instance.release();
        }
        
        private SoundHandle PlayLoopInstance(EventReference sound)
        {
            EventInstance instance = RuntimeManager.CreateInstance(sound);
            instance.start();

            int id = _nextId++;
            _activeSounds.Add(id, instance);

            return new SoundHandle { Id = id };
        }
        
        private void SetParameterInstance(SoundHandle handle, string param, float value)
        {
            if (_activeSounds.TryGetValue(handle.Id, out var instance))
            {
                instance.setParameterByName(param, value);
            }
        }
        
        private void StopInstance(SoundHandle handle, bool fadeOut)
        {
            if (_activeSounds.TryGetValue(handle.Id, out var instance))
            {
                instance.stop(fadeOut ? STOP_MODE.ALLOWFADEOUT : STOP_MODE.IMMEDIATE);
                instance.release();
                _activeSounds.Remove(handle.Id);
            }
        }

        #endregion

        
    }
}
