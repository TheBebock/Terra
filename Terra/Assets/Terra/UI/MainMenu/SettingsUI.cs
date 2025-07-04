using NaughtyAttributes;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Managers;
using Terra.UI.Windows;
using Terra.Utils;
using UIExtensionPackage.UISystem.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.MainMenu
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, ReadOnly] private bool _isInMainMenu = true;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Image _darkScreen;
        [BoxGroup("Audio")] [SerializeField] private  Slider _masterSlider;
        [BoxGroup("Audio")] [SerializeField] private  Slider _sfxSlider;
        [BoxGroup("Audio")] [SerializeField] private  Slider _musicSlider;
        [BoxGroup("Audio")] [SerializeField] private Vector2 _audioRange = new(0, 1);
        private float _lastPlayTime = -1f;
        private float _sfxCooldown = 0.3f;

        [BoxGroup("Gameplay")][SerializeField] private Slider _itemsOpacitySlider;
        [BoxGroup("Gameplay")][SerializeField] private Slider _statsOpacitySlider;
        [BoxGroup("Graphics")] [SerializeField] private Vector2 _opacityRange = new(-0.2f, 0.5f);

        [BoxGroup("Graphics")] [SerializeField] private Button _brightnessButton;
        [BoxGroup("Graphics")] [SerializeField] private GammaSettings _gammaSettings;
        
        
        ItemsOpacityChangedEvent _itemsOpacityChangedEvent;
        StatsOpacityChangedEvent _statsOpacityChangedEvent;
        
        public bool IsInMainMenu { get => _isInMainMenu; set => _isInMainMenu = value; }
        public Button CloseButton => _closeButton;
        private void Awake()
        {
            _itemsOpacityChangedEvent = new ItemsOpacityChangedEvent();
            _statsOpacityChangedEvent = new StatsOpacityChangedEvent();
            
            InitializeSliders();  
            
            _closeButton.onClick.AddListener(OnBackButtonClicked);
            _brightnessButton.onClick.AddListener(OnBrightnessButtonClicked);
        }

        private void Start()
        {
            _gammaSettings.SetSettingsPanel(this);
        }
        

        private void OnBackButtonClicked()
        {
            gameObject.SetActive(false);
            if (UIWindowManager.Instance)
            {
                if(UIWindowManager.Instance.TryGetWindowFromStacks(out PauseWindow pauseWindow))
                {
                    pauseWindow.ShowButtons();
                }
            }
        }
        private void OnBrightnessButtonClicked()
        {
            if(!_isInMainMenu) HideSettings();
            _gammaSettings.EnableGameObject(_isInMainMenu);
        }

        public void ShowSettings()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }

        public void HideSettings()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }
        private void InitializeSliders()
        {
            _masterSlider.onValueChanged.RemoveAllListeners();
            _sfxSlider.onValueChanged.RemoveAllListeners();
            _musicSlider.onValueChanged.RemoveAllListeners();
            
            _statsOpacitySlider.onValueChanged.RemoveAllListeners();
            _itemsOpacitySlider.onValueChanged.RemoveAllListeners();

            SetSliderRange(_masterSlider, _audioRange);
            SetSliderRange(_sfxSlider, _audioRange);
            SetSliderRange(_musicSlider, _audioRange);
            
            SetSliderRange(_itemsOpacitySlider, _opacityRange);
            SetSliderRange(_statsOpacitySlider, _opacityRange);
            
            SetSliderValue(_masterSlider, GameSettings.DefaultMasterVolume);
            SetSliderValue(_sfxSlider, GameSettings.DefaultSFXVolume);
            SetSliderValue(_musicSlider, GameSettings.DefaultMusicVolume);
            
            SetSliderValue(_itemsOpacitySlider, GameSettings.DefaultItemsOpacity);
            SetSliderValue(_statsOpacitySlider, GameSettings.DefaultStatsOpacity);
            
            _masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);

            _sfxSlider.onValueChanged.AddListener((value) =>
            {
                AudioManager.Instance.SetSFXVolume(value);
                PlayTestSFX();
            });

            _musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
            
            _itemsOpacitySlider.onValueChanged.AddListener(OnItemsOpacityChanged);
            _statsOpacitySlider.onValueChanged.AddListener(OnStatsOpacityChanged);
        }




        private void OnItemsOpacityChanged(float value)
        {
            _itemsOpacityChangedEvent.alfa = value;
            EventsAPI.Invoke(ref _itemsOpacityChangedEvent);
        }
        
        private void OnStatsOpacityChanged(float value)
        {
            _statsOpacityChangedEvent.alfa = value;
            EventsAPI.Invoke(ref _statsOpacityChangedEvent);
        }
        

        private void SetSliderRange(Slider slider, Vector2 range)
        {
            slider.minValue = range.x;
            slider.maxValue = range.y;
        }

        private void SetSliderValue(Slider slider, float value)
        {
            slider.value = value;
        }


        public void SetDarkScreenOpacity(float value)
        {
            value = Mathf.Clamp01(value);
            _darkScreen.color = new Color(_darkScreen.color.r, _darkScreen.color.g, _darkScreen.color.b, value);
        }
        private void PlayTestSFX()
        {
            if (Time.time - _lastPlayTime < _sfxCooldown) return;
        
            AudioManager.Instance.PlaySFX("UI_Interaction");
            _lastPlayTime = Time.time;
        }
        
        
        private void OnDisable()
        {
            SetDarkScreenOpacity(1);
            EventsAPI.Invoke<SettingsClosedEvent>();
        }
        
        private void OnValidate()
        {
            if(!_canvasGroup) _canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}