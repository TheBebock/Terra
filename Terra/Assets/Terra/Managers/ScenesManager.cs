using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
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

        public async UniTask<float> LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single,
            bool allowSceneActivation = false, Action<float> onProgress = null, float activationDelay = 0f)
        {

            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, mode);
            if (asyncOp == null)
            {
                Debug.LogError($"{gameObject.name}: Scene not found.");
                return -1;
            }
            asyncOp.allowSceneActivation = allowSceneActivation;

            // Track progress (progress goes up to 0.9 before activation)
            while (!asyncOp.isDone)
            {
                float progress = Mathf.Clamp01(asyncOp.progress / 0.9f);
                onProgress?.Invoke(progress);

                // Scene is loaded, waiting for activation
                if (asyncOp.progress >= 0.9f && !allowSceneActivation)
                {
                    if (activationDelay > 0f)
                        await UniTask.Delay(TimeSpan.FromSeconds(activationDelay), cancellationToken: CancellationToken);

                    asyncOp.allowSceneActivation = true;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, CancellationToken);
            }

            onProgress?.Invoke(1f); // Ensure 100% progress
            return 0;
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
