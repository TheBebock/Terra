using System;
using NaughtyAttributes;
using UnityEngine;

namespace Terra.StateMachine
{
    [Serializable]
    public abstract class BaseState : IState
    {
        
        protected float CrossFadeDuration = 0.1f;
        
        protected BaseState()
        {
            
        }
        
        public virtual void OnEnter()
        {
            // noop
        }

        public virtual void Update()
        {
            // noop
        }

        public virtual void FixedUpdate()
        {
            // noop
        }

        public virtual void OnExit()
        {
            // noop
        }
        
    }   
}