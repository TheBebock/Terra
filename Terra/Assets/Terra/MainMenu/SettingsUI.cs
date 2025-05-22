using Terra.Managers;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;
    public Slider ambientSlider;

    public void Start()
    {
        Debug.Log("Zmieniono glosnosc");
        masterSlider.value = PlayerPrefs.HasKey("MasterVolume") ? PlayerPrefs.GetFloat("MasterVolume") : 10f;
        sfxSlider.value = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : 10f;
        musicSlider.value = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : 10f;
        ambientSlider.value = PlayerPrefs.HasKey("AmbientVolume") ? PlayerPrefs.GetFloat("AmbientVolume") : 1f;

        masterSlider.onValueChanged.RemoveAllListeners();
        masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);

        sfxSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);

        musicSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);

        ambientSlider.onValueChanged.RemoveAllListeners();
        ambientSlider.onValueChanged.AddListener(AudioManager.Instance.SetAmbientVolume);
    }
}