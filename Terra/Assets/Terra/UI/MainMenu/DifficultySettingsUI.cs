using System;
using NaughtyAttributes;
using Terra.Enums;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.MainMenu
{
    public class DifficultySettingsUI : MonoBehaviour
    {
    
        [BoxGroup("Gameplay")][SerializeField] private GameDifficulty _gameDifficulty;
        [BoxGroup("Gameplay")][SerializeField] private Button _difficultyButtonLeft;
        [BoxGroup("Gameplay")][SerializeField] private Button _difficultyButtonRight;
        [BoxGroup("Gameplay")][SerializeField] private TMP_Text _currentDifficultyText;

        [Foldout("Debug")][SerializeField] private int _amountOfDifficulties;
        GameDifficultyChangedEvent _gameDifficultyChanged;

        private void Awake()
        {
            _gameDifficultyChanged = new GameDifficultyChangedEvent();
            _amountOfDifficulties = Enum.GetValues(typeof(GameDifficulty)).Length-1;
            
            
            _difficultyButtonLeft.onClick.AddListener(OnLeftButtonClicked);
            _difficultyButtonRight.onClick.AddListener(OnRightButtonClicked);
        }

        private void OnEnable()
        {
            _gameDifficulty = GameSettings.DefaultDifficultyLevel;
            if (_gameDifficulty <= 0)
            {
                _difficultyButtonLeft.interactable = false;
            }

            if ((int)_gameDifficulty >= _amountOfDifficulties)
            {
                _difficultyButtonRight.interactable = false;
            }
        }

        private void OnLeftButtonClicked()
        {
            _gameDifficulty--;
            if(_gameDifficulty <= 0) _difficultyButtonLeft.interactable = false;
            if ((int)_gameDifficulty < _amountOfDifficulties) _difficultyButtonRight.interactable = true;
            UpdateCurrentDifficulty();
        }

        private void OnRightButtonClicked()
        {
            _gameDifficulty++;
            if((int)_gameDifficulty >= _amountOfDifficulties) _difficultyButtonRight.interactable = false;
            if (_gameDifficulty > 0) _difficultyButtonLeft.interactable = true;
            UpdateCurrentDifficulty();
        }

        private void UpdateCurrentDifficulty()
        {
            _currentDifficultyText.text = _gameDifficulty.ToString();
            _gameDifficultyChanged.difficulty = _gameDifficulty;
            
            GameSettings.DefaultDifficultyLevel = _gameDifficulty;
            
            EventsAPI.Invoke(ref _gameDifficultyChanged);
        }
    }
}
