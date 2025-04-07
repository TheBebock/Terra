using System.Collections;
using System.Collections.Generic;
using Terra.Core.Generics;
using UnityEngine;

namespace Terra.Managers
{

    /// <summary>
    /// Manages loading and unloading scenes
    /// </summary>
    public class SceneManager : PersistentMonoSingleton<SceneManager>
    {

        public void LoadMainMenu()
        {
            TimeManager.Instance?.ResumeTime();
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
