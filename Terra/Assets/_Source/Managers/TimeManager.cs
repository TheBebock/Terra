using System;
using System.Collections;
using System.Collections.Generic;
using Core.Generics;
using UnityEngine;

namespace Managers
{
    public class TimeManager : MonoBehaviourSingleton<TimeManager>
    {
        
        public Action OnTimePaused;
        public Action OnTimeUnpaused;
        public bool IsPaused => Time.timeScale == 0;

        public void Pause()
        {
            Time.timeScale = 0;
            OnTimePaused?.Invoke();
        }

        public void Unpause()
        {
            Time.timeScale = 1;
            OnTimeUnpaused?.Invoke();
        }
    }
}

