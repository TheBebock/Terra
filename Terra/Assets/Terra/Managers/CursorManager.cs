using Terra.Core.Generics;
using UnityEngine;

namespace Terra.Managers
{

    /// <summary>
    /// Handles management of the cursor
    /// </summary>
    public class CursorManager : PersistentMonoSingleton<CursorManager>
    {

        private void SetCursorVisibility(bool isVisible)
        {
            Cursor.visible = isVisible;
        }

        public void SetCursorLockState(bool isLocked)
        {
            if (isLocked) Cursor.lockState = CursorLockMode.Locked;

            else Cursor.lockState = CursorLockMode.Confined;
        }

        public void HideCursor() => SetCursorVisibility(false);
        public void ShowCursor() => SetCursorVisibility(true);
    }
}