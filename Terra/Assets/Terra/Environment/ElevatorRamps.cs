using System.Collections.Generic;
using System.Linq;
using Terra.AI.EnemyStates;
using Terra.Core.Generics;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Interfaces;
using UnityEngine;

namespace Terra.Environment
{
    public sealed class ElevatorRamps : InGameMonobehaviour, IAttachListeners
    {
        [SerializeField] private List<Animator> _animators =new();
        public void AttachListeners()
        {
            EventsAPI.Register<ElevatorGeneratorStartedEvent>(OnGeneratorStarted);
            EventsAPI.Register<ElevatorGeneratorStoppedEvent>(OnGeneratorStopped);
        }

        private void OnGeneratorStarted(ref ElevatorGeneratorStartedEvent @event)
        {
            for (int i = 0; i < _animators.Count; i++)
            {
                _animators[i].CrossFade(AnimationHashes.Idle, 0.1f);
            }
        }

        private void OnGeneratorStopped(ref ElevatorGeneratorStoppedEvent @event)
        {
            for (int i = 0; i < _animators.Count; i++)
            {
                _animators[i].CrossFade(AnimationHashes.Default, 0.1f);
            }
        }
        
        public void DetachListeners()
        {
            EventsAPI.Unregister<ElevatorGeneratorStartedEvent>(OnGeneratorStarted);
            EventsAPI.Unregister<ElevatorGeneratorStoppedEvent>(OnGeneratorStopped);
        }

        private void OnValidate()
        {
            if(_animators.Count == 0) _animators = GetComponentsInChildren<Animator>().ToList();
        }
    }
}
