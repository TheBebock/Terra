using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Terra.Managers
{

    public static class SceneNames
    {
        public const string MainMenu = "MainMenuScene";
        public const string Gameplay = "GameplayScene";
        public const string Loading = "LoadingScene";
    }
    
    /// <summary>
    /// Manages loading and unloading scenes
    /// </summary>
    public class ScenesManager : PersistentMonoSingleton<ScenesManager>
    {
        [SerializeField, ReadOnly] private List<string> _sceneNames = new ();

        public string CurrentSceneName => SceneManager.GetActiveScene().name;

        //TODO: Add async LoadScene, that implements correct initialization pipeline, ie: load ground first, then entities, then ui.. etc

        public void LoadMainMenu()
        {
            TimeManager.Instance?.ResumeTime();
            
            ForceLoadScene(0);
        }

        public void ForceLoadScene(int buildIndex)
        {
            if (buildIndex > SceneManager.sceneCount)
            {
                Debug.LogError("Scene index is out of range.");
                return;
            } 
            
            SceneManager.LoadScene(buildIndex);
        }
        
        public void ForceLoadScene(string sceneName)
        {
            if (!_sceneNames.Contains(sceneName))
            {
                Debug.LogError("Scene name does not exist.");
                return;
            } 
            
            SceneManager.LoadScene(sceneName);
        }

        private void OnValidate()
        {
            if (_sceneNames.Count == 0)
            {
                _sceneNames.Add(SceneNames.MainMenu);
                _sceneNames.Add(SceneNames.Gameplay);
                _sceneNames.Add(SceneNames.Loading);
            }
        }
    }
}
