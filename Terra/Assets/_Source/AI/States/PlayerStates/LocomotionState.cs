using Terra.Player;
using UnityEngine;
using static Terra.Player.PlayerMovement;

namespace Terra.StateMachine.PlayerStates
{
    public class LocomotionState : PlayerBaseState
    {
        private PlayerMoveDirection oldPlayerMoveDirection;
        public LocomotionState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            
        }

        public override void FixedUpdate()
        {
            player.PlayerMovement.HandleMovement();

            if(oldPlayerMoveDirection != player.PlayerMovement.CurrentPlayerMoveDirection)
            {
                oldPlayerMoveDirection = player.PlayerMovement.CurrentPlayerMoveDirection;

                ChangeDirectionOfAnimation();
            }
        }

        private void ChangeDirectionOfAnimation()
        {
            switch (oldPlayerMoveDirection)
            {
                case PlayerMoveDirection.Up: animator.CrossFade(LocomotionUpHash, CrossFadeDuration); break;
                case PlayerMoveDirection.Down: animator.CrossFade(LocomotionDownHash, CrossFadeDuration); break;
                case PlayerMoveDirection.Left: animator.CrossFade(LocomotionLeftHash, CrossFadeDuration); break;
                case PlayerMoveDirection.Right: animator.CrossFade(LocomotionRightHash, CrossFadeDuration); break;
            }
        }
    }
}