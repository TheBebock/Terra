using Terra.Player;
using UnityEngine;

namespace _Source.StateMachine.PlayerStates
{
    public class StunState : PlayerBaseState
    {
        public StunState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.CrossFade(StunHash, CrossFadeDuration);
            //player.PlayerMovement.CanPlayerMove = false;
        }


        public override void OnExit()
        {
            //player.PlayerMovement.CanPlayerMove = true;
        }
    }
}