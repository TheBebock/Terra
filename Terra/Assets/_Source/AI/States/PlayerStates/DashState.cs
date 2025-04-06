using Terra.Player;
using UnityEngine;

namespace _Source.StateMachine.PlayerStates
{
    public class DashState : PlayerBaseState
    {
        public DashState(PlayerManager player, Animator animator) : base(player, animator) 
        {
        }

        public override void OnEnter()
        {
            animator.CrossFade(DashHash, CrossFadeDuration);
        }


        public override void FixedUpdate()
        {
            player.PlayerMovement.HandleMovement();
        }
    }
}


    