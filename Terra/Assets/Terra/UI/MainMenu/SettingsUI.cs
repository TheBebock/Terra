using Terra.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.MainMenu
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        public Slider masterSlider;
        public Slider sfxSlider;
        public Slider musicSlider;
        private float _lastPlayTime = -1f;
        private float _sfxCooldown = 0.3f;
    

        public void Start()
        {
            _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
            
            masterSlider.value = PlayerPrefs.HasKey("MasterVolume") ? PlayerPrefs.GetFloat("MasterVolume") : 10f;
            sfxSlider.value = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : 10f;
            musicSlider.value = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : 10f;

            masterSlider.onValueChanged.RemoveAllListeners();
            masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);

            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.AddListener((value) =>
            {
                AudioManager.Instance.SetSFXVolume(value);
                PlayTestSFX();
            });

            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        }
    
        private void PlayTestSFX()
        {
            if (Time.time - _lastPlayTime < _sfxCooldown) return;
        
            AudioManager.Instance.PlaySFX("UI_Interaction");
            _lastPlayTime = Time.time;
        }
    }
}