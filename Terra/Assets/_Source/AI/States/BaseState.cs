using System;

namespace Terra.StateMachine
{
    [Serializable]
    public abstract class BaseState : IState
    {
        
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