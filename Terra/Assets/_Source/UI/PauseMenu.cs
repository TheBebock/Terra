using Terra.Managers;
using UIExtensionPackage.UISystem.UI.Windows;
using UnityEngine;

namespace Terra.UI
{
    public class PauseMenu : UIWindow
    {

        public GameObject pauseMenu; // Reference to the pause menu UI

        public override bool AllowMultiple { get; } = false;

        
        public void ShowPause()
        {
            pauseMenu.SetActive(true);
            Cursor.visible = true;
        }

        public void HidePause()
        {
            pauseMenu.SetActive(false);
            Cursor.visible = false;
        }

    }
}
