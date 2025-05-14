using UnityEngine;

namespace Terra.Player.PlayerStates
{
    public class DashState : PlayerBaseState
    {
        public DashState(PlayerManager player, Animator animator) : base(player, animator) 
        {
        }

        public override void OnEnter()
        {
            //TODO: Add dash anim
            //animator.CrossFade(DashHash, CrossFadeDuration);
        }


        public override void FixedUpdate()
        {
            player.PlayerMovement.HandleMovement();
        }
    }
}


    