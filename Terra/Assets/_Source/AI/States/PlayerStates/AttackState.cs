using Terra.Player;
using UnityEngine;

namespace Terra.StateMachine.PlayerStates
{
    public class AttackState : PlayerBaseState
    {
        public AttackState(PlayerManager player, Animator animator) : base(player, animator)
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
