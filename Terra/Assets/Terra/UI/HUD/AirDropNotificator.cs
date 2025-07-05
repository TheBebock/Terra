using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System.Collections.Generic;
using Terra.Core.Generics;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Interfaces;
using TMPro;
using UnityEngine;

namespace Terra.UI.HUD
{
    public class AirDropNotificator: InGameMonobehaviour, IAttachListeners
    {
        [SerializeField] private GameObject _airDropNotificatorContainer;
        [SerializeField] private TMP_Text _notificatorText;

        [Foldout("Debug")][SerializeField] private List<float> _assignedTimes = new();
        

        private int _timeDelay = 5;

        private void Awake()
        {
            EventsAPI.Register<OnAirDropSetSpawnDelayEvent>(OnAirDropSetSpawnDelay);
        }
        public void AttachListeners()
        {
            
        }

        private void OnAirDropSetSpawnDelay(ref OnAirDropSetSpawnDelayEvent ev)
        {
            AssignCounting(ev.time);
        }

        public void SetNotificatorTimeText(float time)
        {
            if(_notificatorText == null) return;
            
            if (time < 15)
                _airDropNotificatorContainer.SetActive(true);
            else
                _airDropNotificatorContainer.SetActive(false);

            if (time > 0)
            {
                int minutesDivision = Mathf.FloorToInt(time / 60);
                int secondsDivision = Mathf.FloorToInt(time % 60);
                string minutes = minutesDivision < 10 ? $"0{minutesDivision}" : $"{minutesDivision}";
                string seconds = secondsDivision < 10 ? $"0{secondsDivision}" : $"{secondsDivision}";
            
                string notificatorText = $"{minutes}:{seconds}";

                _notificatorText.text = notificatorText;
            }
            else
            {
                _notificatorText.text = "00:00";
            }
        }

        public void AssignCounting(float time)
        {
            _assignedTimes.Add(time);

            if(_assignedTimes.Count > 1)
            {
                _assignedTimes[_assignedTimes.Count - 1] -= _assignedTimes[0];
            }
            else
            {
                _ = SetNotificatorTime(time);
            }
        }

        private void StartNewCounting(float time)
        {
            _ = SetNotificatorTime(time);
        }

        public async UniTask SetNotificatorTime(float time)
        {
            time += _timeDelay;
            while(time > 0)
            {
                time--;
                _assignedTimes[0] = time;
                SetNotificatorTimeText(time);
                
                await UniTask.WaitForSeconds(1f);
            }
            _assignedTimes.RemoveAt(0);

            if(_assignedTimes.Count > 0)
                StartNewCounting(_assignedTimes[0]);
        }

        


        public void DetachListeners()
        {
            EventsAPI.Unregister<OnAirDropSetSpawnDelayEvent>(OnAirDropSetSpawnDelay);
        }
    }
}