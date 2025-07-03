using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.PostProcess;
using Terra.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.MainMenu
{
    public class GammaSettings : InGameMonobehaviour
    {
        [SerializeField, ReadOnly] private SettingsUI _settingsPanel;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _chestWrapper;
        [SerializeField] private Button _backButton;
        [SerializeField] private Slider _gammaSlider;
        
        private void Start()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            if (VolumesManager.Instance)
            {
                InitSliderValues(VolumesManager.Instance.GammaRange);
                _gammaSlider.value = GameSettings.DefaultGamma;
                _gammaSlider.onValueChanged.AddListener(OnSliderValueChanged);
            }
            else
            {
                Debug.LogError($"{this} Volumes Manager is missing, something went horribly wrong");
            }
        }

        public void SetSettingsPanel(SettingsUI settingsPanel) => _settingsPanel = settingsPanel;
        private void OnBackButtonClicked()
        {
            gameObject.SetActive(false);
            _settingsPanel.ShowSettings();
        }

        public void EnableGameObject(bool isInMainMenu)
        {
            if(isInMainMenu) EnableForMainMenu();
            else EnableForGameplay();
        }
        private void EnableForMainMenu()
        {
            _chestWrapper.SetActive(true);
            gameObject.SetActive(true);
        }

        private void EnableForGameplay()
        {
            _settingsPanel.HideSettings();
            _chestWrapper.SetActive(false);
            gameObject.SetActive(true);
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

        private void OnValidate()
        {
            if(!_canvasGroup) _canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}
