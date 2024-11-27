using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Data.States; 

namespace AI  
{
    public abstract class AIController : MonoBehaviour
    {
        [SerializeField] private AIState[] AIStates; 
        private AIState _previous;  
        private AIState _current;   
        private AIState _next;      
        private AIState _idle;      
        private AIState _death;     
        private AIState _spawn;     

        
        protected void TransitionToState(AIState newState)
        {
            _previous = _current;  
            _current = newState;   
        }

        
        protected virtual void Start()
        {
            spawnBehaviour();

            foreach (AIState state in AIStates)
            {
                if (state.StateName == "Idle") _idle = state;
                if (state.StateName == "Spawn") _spawn = state;
                if (state.StateName == "Death") _death = state;
            }

            if (_spawn != null)
                TransitionToState(_spawn); 
            else
                Debug.LogError("Spawn state is not set in AIStates!");
        }

        
        protected virtual void Update()
        {
            if (_current == null) return; 
            
            switch (_current.StateName)
            {
                case "Idle":
                    idleBehaviour();
                    break;
                case "Spawn":
                    break;
                case "Death":
                    deathBehaviour();
                    break;
                default:
                    Debug.LogWarning($"Unhandled state: {_current.StateName}");
                    break;
            }
        }
        
        protected virtual void idleBehaviour()
        {
            Debug.Log("Default idle behaviour.");
        }
        
        protected virtual void spawnBehaviour()
        {
            Debug.Log("Default spawn behaviour.");
        }
        
        protected virtual void deathBehaviour()
        {
            Debug.Log("Default death behaviour.");
        }
    }
}
