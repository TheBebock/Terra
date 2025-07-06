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
        private int _enviroLevelIndex = 0;
        public void AttachListeners()
        {
            EventsAPI.Register<LevelIncreasedEvent>(OnEndOfFloorAnimEnd);
        }

        private void OnEndOfFloorAnimEnd(ref LevelIncreasedEvent dummy)
        {
            _currentEnvironment.SetActive(false);
            _enviroLevelIndex++;
            _currentEnvironment = _enviroLevels[_enviroLevelIndex-1];
            _currentEnvironment.SetActive(true);
        }

        public void DetachListeners()
        {
            EventsAPI.Unregister<LevelIncreasedEvent>(OnEndOfFloorAnimEnd);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            _currentEnvironment = _enviroLevels[0];
        }
#endif
        
    }
}