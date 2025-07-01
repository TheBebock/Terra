using System;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Interfaces;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace Terra.Managers
{
    public class AudioManager : PersistentMonoSingleton<AudioManager>, IWithSetUp
    {

        [Serializable]
        internal struct Sound
        {
            public string name;
            public AudioClip clip;
        }

        [FormerlySerializedAs("MinDB")] [Foldout("Config")] [SerializeField] private float _minDB;
        [FormerlySerializedAs("MaxDB")] [Foldout("Config")] [SerializeField] private float _maxDB;

        [FormerlySerializedAs("musicSounds")] [Foldout("References")] [SerializeField]
        private Sound[] _musicSounds;

        [FormerlySerializedAs("sfxSounds")] [Foldout("References")] [SerializeField]
        private Sound[] _sfxSounds;

        [FormerlySerializedAs("ambientSounds")] [Foldout("References")] [SerializeField]
        private Sound[] _ambientSounds;

        [FormerlySerializedAs("musicSource")] [Foldout("References")] [SerializeField]
        private AudioSource _musicSource;

        [FormerlySerializedAs("sfxSource")] [Foldout("References")] [SerializeField]
        private AudioSource _sfxSource;

        [FormerlySerializedAs("ambientSource")] [Foldout("References")] [SerializeField]
        private AudioSource _ambientSource;

        [FormerlySerializedAs("AudioMixer")] [Foldout("References")] [SerializeField]
        private AudioMixer _audioMixer;

        [FormerlySerializedAs("MusicAudioLowPassFilter")] [Foldout("References")] [SerializeField]
        private AudioLowPassFilter _musicAudioLowPassFilter;

        [FormerlySerializedAs("AmbientAudioLowPassFilter")] [Foldout("References")] [SerializeField]
        private AudioLowPassFilter _ambientAudioLowPassFilter;
        
        
        public void SetUp()
        {
            
        }

        
        public void PlayMusic(string clip)
        {
            Sound s = Array.Find(_musicSounds, x => x.name == clip);

            if (s.name == default)
            {
                Debug.LogError($"Sound {clip} Not Found");
            }
            else
            {
                _musicSource.clip = s.clip;
                _musicSource.Play();
            }
        }

        public void StopMusic()
        {
            _musicSource.Stop();
        }

        public void PlayAmbient(string clip)
        {
            Sound s = Array.Find(_ambientSounds, x => x.name == clip);

            if (s.name == default)
            {
                Debug.LogError($"Sound {clip} Not Found");
            }
            else
            {
                _ambientSource.clip = s.clip;
                _ambientSource.Play();
            }
        }

        public void StopAmbient(string clip)
        {
            _ambientSource.Stop();
        }

        public void PlaySFX(string clip)
        {
            Sound s = Array.Find(_sfxSounds, x => x.name == clip);

            if (s.name == default)
            {
                Debug.LogError($"Sound {clip} Not Found");
            }
            else
            {
                PlaySFX(s.clip);
            }
        }

        public void PlaySFX(AudioClip clip)
        {
            _sfxSource.PlayOneShot(clip);
        }

        public void PlaySFXAtSource(AudioClip clip, AudioSource source, float pitch = 1f)
        {
            if (source == null)
            {
                Debug.LogError($"{this}: Given source or clip was null");
                return;
            }
            if (clip == null)
            {
                Debug.LogWarning($"{this}: Given clip was null");
                return;
            }
            if (source.isPlaying)
            {
                if(source.clip == clip)
                    return;
            }
            
            source.clip = clip;
            source.volume = _sfxSource.volume;
            source.pitch = pitch;
            source.PlayOneShot(clip);
        }

        //TODO: Possibly refactor
        /// <summary>
        /// Method tries to play SFX, but if its already playing given clip, it won't repeat
        /// </summary>
        public void PlaySFXAtSourceOnce(AudioClip clip, AudioSource source)
        {
            if (source.clip != clip)
            {
                source.clip = clip;
                source.volume = _sfxSource.volume;
                source.PlayOneShot(clip);
            }
            else if (source.clip == clip && source.isPlaying == false)
            {
                source.volume = _sfxSource.volume;
                source.Play();
            }
        }

        public bool IsMusicPlayingClip(string clip)
        {
            Sound s = Array.Find(_musicSounds, x => x.name == clip);
            if (s.name == default)
            {
                Debug.Log("Sound Not Found");
                return false;
            }

            return _musicSource.isPlaying && _musicSource.clip == s.clip;
        }

        private void LoadVolume()
        {
            LoadVolumeSetting("MasterVolume", _minDB, _maxDB);
            LoadVolumeSetting("SFXVolume", _minDB, _maxDB);
            LoadVolumeSetting("MusicVolume", _minDB, _maxDB);
            LoadVolumeSetting("AmbientVolume", _minDB, _maxDB);
        }

        private void LoadVolumeSetting(string parameterName, float minDB, float maxDB)
        {
            float normalizedValue = PlayerPrefs.GetFloat(parameterName, -1f);
            float volume = normalizedValue <= 0 ? -80f : Mathf.Lerp(minDB, maxDB, normalizedValue / 10f);
            _audioMixer.SetFloat(parameterName, volume);
        }
        
        public void EnableMusicLowPassFilter(bool enable)
        {
            _musicAudioLowPassFilter.enabled = enable;
            _ambientAudioLowPassFilter.enabled = enable;
        }

        public void SetMusicPitch(float pitch)
        {
            _musicSource.pitch = pitch;
        }
        private void SetVolume(string parameterName, float value)
        {
            float volume = value == 0 ? -80 : Mathf.Lerp(_minDB, _maxDB, value / 10f);
            _audioMixer.SetFloat(parameterName, volume);
            PlayerPrefs.SetFloat(parameterName, value);
            PlayerPrefs.Save();
        }

        public void SetMasterVolume(float value) => SetVolume("MasterVolume", value);
        public void SetSFXVolume(float value) => SetVolume("SFXVolume", value);
        public void SetMusicVolume(float value) => SetVolume("MusicVolume", value);
        public void SetAmbientVolume(float value) => SetVolume("AmbientVolume", value);


        public void TearDown()
        {
            StopMusic();
        }
    }
    
}
