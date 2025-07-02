using Terra.Core.Generics;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Interfaces;
using Terra.Utils;
using UnityEngine;

namespace Terra.Managers
{
    internal class GameSettingsBootManager : PersistentMonoSingleton<GameSettingsBootManager>, IAttachListeners
    {
        [SerializeField] private int _targetFrameRate = 120;
        protected override void Awake()
        {
            base.Awake();
            if (Instance == this)
            {
                GameSettings.TryLoadingGameSettings();
                Application.targetFrameRate = _targetFrameRate;
            }
        }

        public void AttachListeners()
        {
           EventsAPI.Register<SettingsClosedEvent>(OnSettingsClosedEvent);
        }

        private void OnSettingsClosedEvent(ref SettingsClosedEvent closedEvent)
        {
            GameSettings.SaveGameSettings();
        }
        public void DetachListeners()
        {
            EventsAPI.Unregister<SettingsClosedEvent>(OnSettingsClosedEvent);
        }
    }
}
