using Terra.GameStates;
using Terra.Managers;
using UIExtensionPackage.UISystem.UI.Windows;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Terra.UI.Windows
{
    public class DeadWindow: UIWindow
    {
        public override bool AllowMultiple { get; } = false;

        [FormerlySerializedAs("darkScreen")] [SerializeField] private GameObject _darkScreen;
        [FormerlySerializedAs("playAgainButton")] [SerializeField] private Button _playAgainButton;
        [FormerlySerializedAs("quitButton")] [SerializeField] private Button _quitButton;

        public override void SetUp()
        {
            base.SetUp();

            _playAgainButton?.onClick.AddListener(Restart);
            _quitButton?.onClick.AddListener(ExitGame);

            //TODO: add other functionalities
        }

        private async void Restart()
        {
            _darkScreen.SetActive(true);
            TimeManager.Instance.ResumeTime();
            await ScenesManager.Instance.LoadGameplay();
            GameManager.Instance.SwitchToGameState<StartOfFloorState>();
            Close();
        }

        private void ExitGame() => Application.Quit();

    }
}