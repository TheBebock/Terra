using Terra.GameStates;
using UIExtensionPackage.UISystem.UI.Windows;
using UnityEngine.UI;
using UnityEngine;
using Terra.Managers;

namespace Terra.UI
{
    public class DeadWindow: UIWindow
    {
        public override bool AllowMultiple { get; } = false;

        [SerializeField] private GameObject darkScreen;
        [SerializeField] private Button playAgainButton = default;
        [SerializeField] private Button quitButton = default;

        public override void SetUp()
        {
            base.SetUp();

            playAgainButton?.onClick.AddListener(Restart);
            quitButton?.onClick.AddListener(ExitGame);

            //TODO: add other functionalities
        }

        private async void Restart()
        {
            darkScreen.SetActive(true);
            TimeManager.Instance.ResumeTime();
            await ScenesManager.Instance.LoadSceneAsync(SceneNames.Gameplay, activationDelay:1f);
            GameManager.Instance?.SwitchToGameState<StartOfFloorState>();
            Close();
        }

        private void ExitGame() => Application.Quit();

    }
}