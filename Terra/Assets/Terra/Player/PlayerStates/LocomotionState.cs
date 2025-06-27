using Terra.Enums;
using UnityEngine;

namespace Terra.Player.PlayerStates
{
    public class LocomotionState : PlayerBaseState
    {
        private FacingDirection _oldPlayerMoveDirection;
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

            if(_oldPlayerMoveDirection != player.PlayerMovement.CurrentPlayerMoveDirection)
            {
                _oldPlayerMoveDirection = player.PlayerMovement.CurrentPlayerMoveDirection;

                ChangeDirectionOfAnimation();
            }
        }

        private void ChangeDirectionOfAnimation()
        {
            switch (_oldPlayerMoveDirection)
            {
                case FacingDirection.Up: animator.CrossFade(LocomotionUpHash, crossFadeDuration); break;
                case FacingDirection.Down: animator.CrossFade(LocomotionDownHash, crossFadeDuration); break;
                case FacingDirection.Left: animator.CrossFade(LocomotionLeftHash, crossFadeDuration); break;
                case FacingDirection.Right: animator.CrossFade(LocomotionRightHash, crossFadeDuration); break;
            }
        }
    }
}