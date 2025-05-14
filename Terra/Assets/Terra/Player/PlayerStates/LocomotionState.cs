using Terra.Enums;
using UnityEngine;

namespace Terra.Player.PlayerStates
{
    public class LocomotionState : PlayerBaseState
    {
        private FacingDirection oldPlayerMoveDirection;
        public LocomotionState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            ChangeDirectionOfAnimation();
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
                case FacingDirection.Up: animator.CrossFade(LocomotionUpHash, CrossFadeDuration); break;
                case FacingDirection.Down: animator.CrossFade(LocomotionDownHash, CrossFadeDuration); break;
                case FacingDirection.Left: animator.CrossFade(LocomotionLeftHash, CrossFadeDuration); break;
                case FacingDirection.Right: animator.CrossFade(LocomotionRightHash, CrossFadeDuration); break;
            }
        }
    }
}