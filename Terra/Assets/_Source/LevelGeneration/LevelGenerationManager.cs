using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Managers;
using Terra.UI;
using UIExtensionPackage.UISystem.UI.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Terra.LevelGeneration
{
    /// <summary>
    /// Handles changing of the levels
    /// </summary>
    public class LevelGenerationManager : PersistentMonoSingleton<LevelGenerationManager>
    {
        //[SerializeField] private ScenesDatabase _scenesDatabase;

        [SerializeField] List<LevelMapContainer> levelMaps = new();

        [SerializeField, ReadOnly] private int currentLevel = 0;

        public event Action OnLevelLoaded;
        
        public void LoadNewLevel()
        {
            if (currentLevel >= levelMaps.Count - 1) return;

            TimeManager.Instance?.PauseTime();
            ShowUpgradePanel();

            UnloadCurrentLevel();
            PrepareNewLevel();

            OnLevelLoaded?.Invoke();
        }

        private void PrepareNewLevel()
        {
            levelMaps[currentLevel + 1]?.gameObject.SetActive(true);
            EnemyManager.Instance.GenerateEnemies();

            currentLevel++;
        }

        private void UnloadCurrentLevel()
        {
            levelMaps[currentLevel].gameObject.SetActive(false);
        }

        private void ShowUpgradePanel()
        {
            UIWindowManager.Instance?.OpenWindow<RewardWindow>();

        }

        // TODO: Delete this when all tests will be done
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                LoadNewLevel();
            }
        }
    }
}