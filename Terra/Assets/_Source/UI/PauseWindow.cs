using Terra.Managers;
using UIExtensionPackage.UISystem.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI
{
    public class PauseWindow : UIWindow
    {
        public override bool AllowMultiple { get; } = false;
        
        [SerializeField] Button resumeButton;
        [SerializeField] Button saveButton;
        [SerializeField] Button loadButton;
        [SerializeField] Button optionsButton;
        [SerializeField] Button exitToMenuButton;

        public override void SetUp()
        {
            base.SetUp();
            
            resumeButton?.onClick.AddListener(Resume);
            exitToMenuButton?.onClick.AddListener(ExitGame);
            
            //TODO: add other functionalities
        }

        private void Resume()
        {
            TimeManager.Instance?.ResumeGame();
            Close();
        }
        
        private void ExitGame() => Application.Quit();
    }
}
