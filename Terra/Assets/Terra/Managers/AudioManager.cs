using System;
using NaughtyAttributes;
using Terra.Core.Generics;
using UnityEngine;
using UnityEngine.Audio;

namespace Terra.Managers
{
    public class AudioManager : PersistentMonoSingleton<AudioManager>
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

        //TODO: Normal saving settings
        private void LoadVolume()
        {
            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                float volume = 0f;
                if (PlayerPrefs.GetFloat("MasterVolume") == 0)
                    volume = -80;
                else
                    volume = Mathf.Lerp(MinDB, MaxDB, PlayerPrefs.GetFloat("MasterVolume") / 10f);

                AudioMixer.SetFloat("MasterVolume", volume);
            }
            else
            {
                float volume = Mathf.Lerp(MinDB, MaxDB, -1f / 10f);
                AudioMixer.SetFloat("MasterVolume", volume);

            }

            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                float volume = 0f;
                if (PlayerPrefs.GetFloat("SFXVolume") == 0)
                    volume = -80;
                else
                    volume = Mathf.Lerp(MinDB, MaxDB, PlayerPrefs.GetFloat("SFXVolume") / 10f);

                AudioMixer.SetFloat("SFXVolume", volume);
            }
            else
            {
                float volume = Mathf.Lerp(MinDB, MaxDB, -1f / 10f);
                AudioMixer.SetFloat("SFXVolume", volume);

            }

            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                float volume = 0f;
                if (PlayerPrefs.GetFloat("MusicVolume") == 0)
                    volume = -80;
                else
                    volume = Mathf.Lerp(MinDB, MaxDB, PlayerPrefs.GetFloat("MusicVolume") / 10f);

                AudioMixer.SetFloat("MusicVolume", volume);
            }
            else
            {
                float volume = Mathf.Lerp(MinDB, MaxDB, -1f / 10f);
                AudioMixer.SetFloat("MusicVolume", volume);

            }
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
    }
}
