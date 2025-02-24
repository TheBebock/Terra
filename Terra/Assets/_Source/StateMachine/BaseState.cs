using UnityEngine;

namespace _Source.StateMachine
{
    // TODO Add PlayerController here 
    public abstract class BaseState : IState
    {
        protected readonly PlayerController Player;
        protected readonly Animator Animator;
    
        protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int JumpHash = Animator.StringToHash("Jump");
    
        protected const float CrossFadeDuration = 0.1f;

        protected BaseState(PlayerController player, Animator animator)
        {
            this.Player = player;
            this.Animator = animator;
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
    // Just to remove the  NULL error I will fix this later (maybe)
    public class PlayerController
    {
    }
}