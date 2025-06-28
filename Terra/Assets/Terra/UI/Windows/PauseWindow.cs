using Terra.Managers;
using UIExtensionPackage.UISystem.UI.Windows;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Terra.UI.Windows
{
    public class PauseWindow : UIWindow
    {
        public override bool AllowMultiple { get; } = false;
        
        [FormerlySerializedAs("resumeButton")] [SerializeField] Button _resumeButton;
        [FormerlySerializedAs("saveButton")] [SerializeField] Button _saveButton;
        [FormerlySerializedAs("loadButton")] [SerializeField] Button _loadButton;
        [FormerlySerializedAs("optionsButton")] [SerializeField] Button _optionsButton;
        [FormerlySerializedAs("exitToMenuButton")] [SerializeField] Button _exitToMenuButton;

        public override void SetUp()
        {
            base.SetUp();
            
            _resumeButton?.onClick.AddListener(Resume);
            _exitToMenuButton?.onClick.AddListener(ExitGame);
            
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
