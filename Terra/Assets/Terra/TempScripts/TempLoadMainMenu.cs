using JetBrains.Annotations;
using Terra.Managers;
using UnityEngine;

namespace Terra.TempScripts
{
    public class TempLoadMainMenu : MonoBehaviour
    {
        [UsedImplicitly]
        public void LoadMainMenu()
        {
            ScenesManager.Instance.LoadMainMenu();
        }
    }
}
