using Terra.Core.Generics;
using Terra.Interfaces;
using UnityEngine;

namespace Terra.Managers
{

    /// <summary>
    /// Handles management of the cursor
    /// </summary>
    public class CursorManager : PersistentMonoSingleton<CursorManager>, IWithSetUp
    {
        [SerializeField] Texture2D _cursorTexture;
        public void SetUp()
        {
            SetCursorTexture();
        }

        private void SetCursorTexture()
        {
            Vector2 textureOffset = new(_cursorTexture.width * 0.5f, _cursorTexture.height * 0.5f);
            Cursor.SetCursor(_cursorTexture, textureOffset, CursorMode.Auto);   
        }

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

        public void TearDown()
        {
            //Noop
        }
    }
}