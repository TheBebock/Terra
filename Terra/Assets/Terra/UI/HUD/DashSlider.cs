using System;
using NaughtyAttributes;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using UIExtensionPackage.UISystem.Core.Base;
using UIExtensionPackage.UISystem.Core.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.UI.HUD
{
    [RequireComponent(typeof(Slider))]
    public class DashSlider : UIObject, IAttachListeners
    {
        [SerializeField] private Slider _dashSlider;
        [SerializeField] private Image _dashReadyMask;
        
        public void AttachListeners()
        {
            EventsAPI.Register<OnPlayerDashTimerProgressedEvent>(OnPlayerDashTimerProgressed);
        }

        private void OnPlayerDashTimerProgressed(ref OnPlayerDashTimerProgressedEvent ev)
        { 
            float mappedProgress = 1 - ev.progress;
            _dashSlider.value = mappedProgress;
            
            if (ev.progress <= 0)
            {
                OnDashReady();
            }
            else
            {
                OnDashNotReady();
            }
        }
        private void OnDashReady()
        {
            _dashReadyMask.gameObject.SetActive(true);
        }
        private void OnDashNotReady()
        {
            _dashReadyMask.gameObject.SetActive(false);
        }
        public void DetachListeners()
        {
            EventsAPI.Unregister<OnPlayerDashTimerProgressedEvent>(OnPlayerDashTimerProgressed);
        }

        private void OnValidate()
        {
            if(!_dashSlider) _dashSlider = GetComponent<Slider>();
        }
    }
}
