using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateWeight
{
    Lowest,
    Low,
    Medium,
    High,
    Highest
}

namespace AI.Data.States
{
    [System.Serializable]
    public abstract class AIState : ScriptableObject
    {
        public bool isDefault;
        
        public List<AIState> exitTransitionalStates = new List<AIState>(); 
        
        public StateWeight StateWeight;
        public abstract void StartState(AIController controller);

        public abstract void UpdateState(AIController controller);

        public abstract void ExitState(AIController controller);

        public abstract bool CanExitState(AIController controller);

        public abstract bool CanEnterState(AIController controller);
        
    }
}