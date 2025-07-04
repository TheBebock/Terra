using System;
using Cysharp.Threading.Tasks;
using Terra.Core.Generics;
using Terra.GameStates;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Terra.Managers
{

    public static class SceneNames
    {
        public const string IntroScene = "IntroScene";
        public const string MainMenu = "MainMenuScene";
        public const string Gameplay = "GameplayScene";
        public const string Loading = "LoadingScene";
        public const string OutroScene = "OutroScene";
    }

    /// <summary>
    /// Manages loading and unloading scenes
    /// </summary>
    public class ScenesManager : PersistentMonoSingleton<ScenesManager>
    {
        public string CurrentSceneName => SceneManager.GetActiveScene().name;

        public async void LoadMainMenu()
        {
            await LoadSceneAsync(SceneNames.MainMenu, LoadSceneMode.Single, true);
            GameManager.Instance?.SwitchToGameState<DefaultGameState>();
            AudioManager.Instance?.PlayMusic("main_menu");
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

        public async UniTask LoadGameplay()
        {
            await LoadSceneAsync(SceneNames.Loading, allowSceneActivation: true);
            await LoadSceneAsync(SceneNames.Gameplay, activationDelay: 1f);
            GameManager.Instance?.SwitchToGameState<StartOfFloorState>();
        }

        public async UniTask LoadOutro()
        {
            await LoadSceneAsync(SceneNames.Loading, allowSceneActivation: true);
            await LoadSceneAsync(SceneNames.OutroScene, activationDelay: 1f);
            
        }

        public async UniTask<float> LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single,
            bool allowSceneActivation = false, Action<float> onProgress = null, float activationDelay = 0f)
        {

            TimeManager.Instance?.ResumeTime();

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
                        await UniTask.Delay(TimeSpan.FromSeconds(activationDelay),
                            cancellationToken: CancellationToken);

                    asyncOp.allowSceneActivation = true;
                }

                await UniTask.Yield(PlayerLoopTiming.Update, CancellationToken);
            }

            onProgress?.Invoke(1f); // Ensure 100% progress
            return 0;
        }
    }
}
