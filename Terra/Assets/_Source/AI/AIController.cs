using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Data.States;
using Unity.VisualScripting;

namespace AI
{
    public abstract class AIController : MonoBehaviour
    {
        [SerializeField] private List<AIState> AIStates;
        private AIState _previous;
        private AIState _current;
        private AIState _next;


        protected void TransitionToState(AIState newState)
        {
            if (!newState) return;

            if (!CanTransitionToStates(_current.exitTransitionalStates, out List<AIState> transitionStates)) return;

            _previous = _current;
            _current = newState;
        }

        public bool CanTransitionToStates(List<AIState> aiStates, out List<AIState> result)
        {
            result = new();
            for (int i = 0; i < AIStates.Count; i++)
            {
                AIState state = AIStates[i];

                if (aiStates.Contains(state))
                {
                    result.Add(state);
                }
            }

            return result.Count > 0;
        }

        public bool CanEnterStates(List<AIState> aiStates, out List<AIState> result)
        {
            result = new();
            for (int i = 0; i < aiStates.Count; i++)
            {
                AIState state = aiStates[i];

                if (state.CanEnterState(this))
                {
                    result.Add(state);
                }
            }

            return result.Count > 0;
        }

        public void TryGetBiggestWeight(List<AIState> aiStates, out List<AIState> result)
        {
            result = new();
            StateWeight highestWeight = StateWeight.Lowest;
            for (int i = 0; i < aiStates.Count; i++)
            {
                AIState state = aiStates[i];

                if (state.StateWeight > highestWeight)
                {
                    highestWeight = state.StateWeight;
                }
            }
            
            for(int i = 0; i<aiStates.Count; i++)
            {
                AIState state = aiStates[i];

                if (state.StateWeight == highestWeight)
                {
                    result.Add(state);
                }
            }
        }
        
    }
}