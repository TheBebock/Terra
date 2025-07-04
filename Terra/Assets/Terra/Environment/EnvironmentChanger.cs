using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Interfaces;
using UnityEngine;

namespace Terra.Environment
{
    /// <summary>
    /// Handles changing of the levels
    /// </summary>
    public class EnvironmentChanger : InGameMonobehaviour, IAttachListeners
    {
        [SerializeField] List<GameObject> _enviroLevels = new();

        [SerializeField, ReadOnly] GameObject _currentEnvironment;
        private int _enviroLevelIndex;
        public void AttachListeners()
        {
            EventsAPI.Register<EndOfElevatorAnimationEvent>(OnEndOfFloorAnimEnd);
        }

        private void OnEndOfFloorAnimEnd(ref EndOfElevatorAnimationEvent dummy)
        {
            _currentEnvironment.SetActive(false);
            _enviroLevelIndex++;
            _currentEnvironment = _enviroLevels[_enviroLevelIndex];
            _currentEnvironment.SetActive(true);
        }

        public void DetachListeners()
        {
            EventsAPI.Unregister<EndOfElevatorAnimationEvent>(OnEndOfFloorAnimEnd);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _currentEnvironment = _enviroLevels[0];
        }
#endif
        
    }
}