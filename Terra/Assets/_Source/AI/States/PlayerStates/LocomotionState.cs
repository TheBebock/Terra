using _Source.Player;
using Terra.Player;
using UnityEngine;

namespace _Source.StateMachine.PlayerStates
{
    public class LocomotionState : PlayerBaseState
    {
        protected LocomotionState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.CrossFade(LocomotionHash, CrossFadeDuration);
        }

        public override void FixedUpdate()
        {
            //call player's move logic 
        }
    }
}