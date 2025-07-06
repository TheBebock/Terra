using System;
using NaughtyAttributes;
using Terra.Enums;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Managers;
using Terra.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.MainMenu
{
    public class FirstGameDifficultyPanel : MonoBehaviour
    {
        [Serializable]
        internal struct DifficultyDescriptionData
        {
            public GameDifficulty difficulty;
            public string description;
        }
        
        [SerializeField] private DifficultyDescriptionData[] _difficultyDescriptions;
        
        [SerializeField] private ToggleGroup _toggleGroup;
        [SerializeField] private Toggle _cyberiadaToggle;
        [SerializeField] private Toggle _easyToggle;
        [SerializeField] private Toggle _normalToggle;
        [SerializeField] private Button _confirmButton;
        
        [SerializeField] private TMP_Text _currentDifficultyDescriptionText;
        
        [SerializeField, ReadOnly] private GameDifficulty _gameDifficulty;

        GameDifficultyChangedEvent _gameDifficultyChanged;

        private void Awake()
        {
            _gameDifficultyChanged = new GameDifficultyChangedEvent();
            _gameDifficulty = GameSettings.DefaultDifficultyLevel;
            
            _cyberiadaToggle?.onValueChanged.AddListener(OnCyberiadaToggleClicked);
            _easyToggle?.onValueChanged.AddListener(OnEasyToggleClicked);
            _normalToggle?.onValueChanged.AddListener(OnNormalToggleClicked);
            _confirmButton?.onClick.AddListener(OnConfirmButtonClicked);
        }

        private void OnCyberiadaToggleClicked(bool value)
        {
            if (value)
            {
                UpdateCurrentDifficulty(GameDifficulty.Cyberiada);
            }
            else
            {
                if (!_toggleGroup.AnyTogglesOn())
                {
                    _confirmButton.interactable = false;
                    _currentDifficultyDescriptionText.text = string.Empty;
                }
            }
        }
        
        private void OnEasyToggleClicked(bool value)
        {
            if (value)
            {
                UpdateCurrentDifficulty(GameDifficulty.Easy);
            }
            else
            {
                if (!_toggleGroup.AnyTogglesOn())
                {
                    _confirmButton.interactable = false;
                    _currentDifficultyDescriptionText.text = string.Empty;
                }
            }
        }
        private void OnNormalToggleClicked(bool value)
        {
            if (value)
            {
                UpdateCurrentDifficulty(GameDifficulty.Normal);
            }
            else
            {
                if (!_toggleGroup.AnyTogglesOn())
                {
                    _confirmButton.interactable = false;
                    _currentDifficultyDescriptionText.text = string.Empty;
                }
            }
        }
        
        private string GetDifficultyDescription()
        {
            for (int i = 0; i < _difficultyDescriptions.Length; i++)
            {
                if (_difficultyDescriptions[i].difficulty == _gameDifficulty)
                {
                    return _difficultyDescriptions[i].description;
                }
            }
            
            return string.Empty;
        }
        private void UpdateCurrentDifficulty(GameDifficulty newDifficulty)
        {
            _gameDifficulty = newDifficulty;
            _currentDifficultyDescriptionText.text = GetDifficultyDescription();
            _gameDifficultyChanged.difficulty = _gameDifficulty;
            _confirmButton.interactable = true;
        }

        private void OnConfirmButtonClicked()
        {
            GameSettings.DefaultDifficultyLevel = _gameDifficulty;
            GameSettings.IsFirstEverGame = false;
            GameSettings.SaveGameSettings();
            EventsAPI.Invoke(ref _gameDifficultyChanged);
            
            ScenesManager.Instance?.LoadGameplay();
        }
    }
}
