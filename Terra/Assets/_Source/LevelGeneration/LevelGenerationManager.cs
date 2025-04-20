using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Terra.LevelGeneration
{
    /// <summary>
    /// Handles changing of the levels
    /// </summary>
    public class LevelGenerationManager : PersistentMonoSingleton<LevelGenerationManager>
    {
        [SerializeField] private ScenesDatabase _scenesDatabase;

        [SerializeField, ReadOnly] private int currentLevel = 0;

        public event Action OnLevelLoaded;
        
        
    }
}