using System.Collections.Generic;
using Terra.Core.Generics;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField] private List<string> sceneNames = new List<string>();

        public string CurrentSceneName => SceneManager.GetActiveScene().name;
        protected override void Awake()
        {
            base.Awake();
            sceneNames.Add(SceneNames.MainMenu);
            sceneNames.Add(SceneNames.Gameplay);
            sceneNames.Add(SceneNames.Loading);
        }

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
            if (!sceneNames.Contains(sceneName))
            {
                Debug.LogError("Scene name does not exist.");
                return;
            } 
            
            SceneManager.LoadScene(sceneName);
        }
    }
}
