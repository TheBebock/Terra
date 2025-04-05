using System;
using System.Collections.Generic;
using Terra.Core.Generics;
using UnityEngine;

namespace Terra.LevelGeneration
{
    /// <summary>
    /// Handles changing of the levels
    /// </summary>
    public class LevelGenerationManager : PersistentMonoSingleton<LevelGenerationManager>
    {
        [SerializeField] private List<GameObject> levelPrefabs;

        [SerializeField] private GameObject currentLevel;

        public event Action OnLevelLoaded;

        public void LoadNewLevel()
        {
            UnloadCurrentLevel();
            GameObject currentLevelPO = levelPrefabs.PopRandomElement<GameObject>();
            currentLevel = Instantiate(currentLevelPO, this.transform);
            OnLevelLoaded?.Invoke();
        }

        public void UnloadCurrentLevel()
        {
            if (currentLevel)
                Destroy(currentLevel);
        }
    }
}