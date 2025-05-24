using UIExtensionPackage.UISystem.UI.Windows;
using UnityEngine.UI;
using UnityEngine;
using Terra.Managers;

namespace Terra.UI
{
    public class DeadWindow: UIWindow
    {
        public override bool AllowMultiple { get; } = false;


        [SerializeField] private Button playAgainButton = default;
        [SerializeField] private Button quitButton = default;

        public override void SetUp()
        {
            base.SetUp();

            playAgainButton?.onClick.AddListener(Resume);
            quitButton?.onClick.AddListener(ExitGame);

            //TODO: add other functionalities
        }

        private void Resume()
        {
            TimeManager.Instance?.ResumeTime();
            InputManager.Instance?.SetPlayerControlsState(true);
            ScenesManager.Instance?.ForceLoadScene(SceneNames.Gameplay);  
            Close();
        }

        private void ExitGame() => Application.Quit();

    }
}