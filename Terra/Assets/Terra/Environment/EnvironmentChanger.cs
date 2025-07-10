using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using UnityEngine;

namespace Terra.Environment
{
    /// <summary>
    /// Handles changing of the levels
    /// </summary>
    public class EnvironmentChanger : InGameMonobehaviour
    {
        [SerializeField] List<GameObject> _enviroLevels = new();

        [SerializeField, ReadOnly] GameObject _currentEnvironment;
        [SerializeField] private int _enviroLevelIndex = -1;

        private void Awake()
        {
            EventsAPI.Register<LevelIncreasedEvent>(OnEndOfFloorAnimEnd);
        }

        private void OnEndOfFloorAnimEnd(ref LevelIncreasedEvent dummy)
        {
            _currentEnvironment.SetActive(false);
            _enviroLevelIndex++;
            _enviroLevelIndex = Mathf.Clamp(_enviroLevelIndex, 0, _enviroLevels.Count - 1);
            _currentEnvironment = _enviroLevels[_enviroLevelIndex];
            _currentEnvironment.SetActive(true);
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            EventsAPI.Unregister<LevelIncreasedEvent>(OnEndOfFloorAnimEnd);
        }
    }
}