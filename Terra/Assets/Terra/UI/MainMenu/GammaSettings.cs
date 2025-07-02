using System;
using Terra.Core.Generics;
using Terra.PostProcess;
using Terra.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.MainMenu
{
    public class GammaSettings : InGameMonobehaviour
    {
    
        [SerializeField] private Button _backButton;
        [SerializeField] private Slider _gammaSlider;
        
        private void Start()
        {
            _backButton.onClick.AddListener(()=> gameObject.SetActive(false));
            if (VolumesManager.Instance)
            {
                InitSliderValues(VolumesManager.Instance.GammaRange);
                _gammaSlider.onValueChanged.AddListener(OnSliderValueChanged);
            }
            else
            {
                Debug.LogError($"{this} Volumes Manager is missing, something went horribly wrong");
            }
            
            _gammaSlider.value = GameSettings.DefaultGamma;
        }

        private void OnEnable()
        {
            _gammaSlider.value = GameSettings.DefaultGamma;
        }

        private void InitSliderValues(Vector2 gammaRange)
        {
            _gammaSlider.minValue = gammaRange.x;
            _gammaSlider.maxValue = gammaRange.y;
        }
        private void OnSliderValueChanged(float value)
        {
            VolumesManager.Instance?.SetGamma(value);
        }
    }
}
