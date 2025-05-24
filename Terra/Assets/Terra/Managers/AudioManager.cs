using System;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Interfaces;
using UnityEngine;
using UnityEngine.Audio;

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

        [Foldout("Config")] [SerializeField] private float MinDB;
        [Foldout("Config")] [SerializeField] private float MaxDB;

        [Foldout("References")] [SerializeField]
        private Sound[] musicSounds, sfxSounds, ambientSounds;

        [Foldout("References")] [SerializeField]
        private AudioSource musicSource, sfxSource, ambientSource;

        [Foldout("References")] [SerializeField]
        private AudioMixer AudioMixer;

        [Foldout("References")] [SerializeField]
        private AudioLowPassFilter MusicAudioLowPassFilter;

        [Foldout("References")] [SerializeField]
        private AudioLowPassFilter AmbientAudioLowPassFilter;
        
        
        
        public void PlayMusic(string clip)
        {
            Sound s = Array.Find(musicSounds, x => x.name == clip);

            if (s.name == default)
            {
                Debug.LogError($"Sound {clip} Not Found");
            }
            else
            {
                musicSource.clip = s.clip;
                musicSource.Play();
            }
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void PlayAmbient(string clip)
        {
            Sound s = Array.Find(ambientSounds, x => x.name == clip);

            if (s.name == default)
            {
                Debug.LogError($"Sound {clip} Not Found");
            }
            else
            {
                ambientSource.clip = s.clip;
                ambientSource.Play();
            }
        }

        public void StopAmbient(string clip)
        {
            ambientSource.Stop();
        }

        public void PlaySFX(string clip)
        {
            Sound s = Array.Find(sfxSounds, x => x.name == clip);

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
            sfxSource.PlayOneShot(clip);
        }

        public void PlaySFXAtSource(AudioClip clip, AudioSource source, float pitch = 1f)
        {
                if (source.isPlaying)
            {
                if(source.clip == clip)
                    return;
            }
            
            source.clip = clip;
            source.volume = sfxSource.volume;
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
                source.volume = sfxSource.volume;
                source.PlayOneShot(clip);
            }
            else if (source.clip == clip && source.isPlaying == false)
            {
                source.volume = sfxSource.volume;
                source.Play();
            }
        }

        public bool IsMusicPlayingClip(string clip)
        {
            Sound s = Array.Find(musicSounds, x => x.name == clip);
            if (s.name == default)
            {
                Debug.Log("Sound Not Found");
                return false;
            }

            return musicSource.isPlaying && musicSource.clip == s.clip;
        }

        private void LoadVolume()
        {
            LoadVolumeSetting("MasterVolume", MinDB, MaxDB);
            LoadVolumeSetting("SFXVolume", MinDB, MaxDB);
            LoadVolumeSetting("MusicVolume", MinDB, MaxDB);
            LoadVolumeSetting("AmbientVolume", MinDB, MaxDB);
        }

        private void LoadVolumeSetting(string parameterName, float minDB, float maxDB)
        {
            float normalizedValue = PlayerPrefs.GetFloat(parameterName, -1f);
            float volume = normalizedValue <= 0 ? -80f : Mathf.Lerp(minDB, maxDB, normalizedValue / 10f);
            AudioMixer.SetFloat(parameterName, volume);
        }
        
        public void EnableMusicLowPassFilter(bool enable)
        {
            MusicAudioLowPassFilter.enabled = enable;
            AmbientAudioLowPassFilter.enabled = enable;
        }

        public void SetMusicPitch(float pitch)
        {
            musicSource.pitch = pitch;
        }
        private void SetVolume(string parameterName, float value)
        {
            float volume = value == 0 ? -80 : Mathf.Lerp(MinDB, MaxDB, value / 10f);
            AudioMixer.SetFloat(parameterName, volume);
            PlayerPrefs.SetFloat(parameterName, value);
            PlayerPrefs.Save();
        }

        public void SetMasterVolume(float value) => SetVolume("MasterVolume", value);
        public void SetSFXVolume(float value) => SetVolume("SFXVolume", value);
        public void SetMusicVolume(float value) => SetVolume("MusicVolume", value);
        public void SetAmbientVolume(float value) => SetVolume("AmbientVolume", value);

        public void SetUp()
        {
            PlayMusic("Music");
        }

        public void TearDown()
        {
            StopMusic();
        }
    }
    
}
