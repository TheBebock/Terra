using System.Collections.Generic;
using System.Linq;
using Terra.AI.EnemyStates;
using Terra.Components;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.GameStates;
using Terra.Interfaces;
using Terra.Managers;
using UnityEngine;

namespace Terra.Interactions
{
    public class ElevatorEngine : InteractableBase, IAttachListeners
    {
        [SerializeField] private Light _redLight;
        [SerializeField] private LightPulse _greenLight;
        [SerializeField] private List<Animator> _animators;
        
        public void AttachListeners()
        {
            EventsAPI.Register<WaveEndedEvent>(OnWaveEnded);
            EventsAPI.Register<ElevatorGeneratorStoppedEvent>(OnGeneratorStopped);
        }
        
        public override void OnInteraction()
        {
            Debug.Log("Generator OnInteraction");
            
            ChangeInteractibility(false);
            
            StartAnimations();
            _greenLight.StopLightMode();
            EventsAPI.Invoke<ElevatorGeneratorStartedEvent>();
            GameManager.Instance.SwitchToGameState<EndOfFloorState>();
        }

        protected override void OnInteractableStateChanged(bool interactableState)
        {
            base.OnInteractableStateChanged(interactableState);
            if (interactableState)
            {
                _greenLight?.gameObject.SetActive(true);
                _redLight?.gameObject.SetActive(false);
            }
            else
            {
                _greenLight?.gameObject.SetActive(false);
                _redLight?.gameObject.SetActive(true);
            }
        }

        private void StartAnimations()
        {
            for (int i = 0; i < _animators.Count; i++)
            {
                _animators[i].CrossFade(AnimationHashes.Idle, 0.1f);
            }
        }

        private void StopAnimations()
        {
            for (int i = 0; i < _animators.Count; i++)
            {
                _animators[i].CrossFade(AnimationHashes.Default, 0.1f);
            }
        }
        private void OnWaveEnded(ref WaveEndedEvent @event)
        {
            ChangeInteractibility(true);
            _greenLight.StartLightMode();
        }
        
        private void OnGeneratorStopped(ref ElevatorGeneratorStoppedEvent @event)
        {
            ChangeInteractibility(false);
            StopAnimations();
        }

        public void DetachListeners()
        {
            EventsAPI.Unregister<WaveEndedEvent>(OnWaveEnded);
            EventsAPI.Unregister<ElevatorGeneratorStoppedEvent>(OnGeneratorStopped);
        }

#if UNITY_EDITOR
        
        protected override void OnValidate()
        {
            base.OnValidate();
            if(_animators.Count == 0)_animators = GetComponentsInChildren<Animator>().ToList();
        }
#endif

    }
}
