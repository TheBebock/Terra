using UnityEngine;

namespace Terra.Player.PlayerStates
{
    public class StunState : PlayerBaseState
    {
        public StunState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.CrossFade(StunHash, crossFadeDuration);
            //player.PlayerMovement.CanPlayerMove = false;
        }


        public override void OnExit()
        {
            //player.PlayerMovement.CanPlayerMove = true;
        }
    }
}