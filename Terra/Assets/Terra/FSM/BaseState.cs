using System;
using NaughtyAttributes;
using UnityEngine;

namespace Terra.FSM
{
    [Serializable]
    public abstract class BaseState : IState
    {
        [SerializeField, ReadOnly] private string _name;

        protected BaseState()
        {
            _name = GetType().Name;
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