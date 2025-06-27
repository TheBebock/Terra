using Terra.Enums;
using Terra.FSM;
using UnityEngine;

namespace Terra.Player.PlayerStates
{
    public class IdleState : PlayerBaseState
    {
        private IState _lastState;
        public IdleState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            _lastState = player.GetLastState();
            ChangeDirectionOfAnimation();
        }

        private void ChangeDirectionOfAnimation()
        {
            if (_lastState == null)
            {
                animator.CrossFade(IdleDownHash, crossFadeDuration);
                return;
            }

            if (_lastState.GetType() == typeof(LocomotionState)) UseMovementDirection();
            if (_lastState.GetType() == typeof(MeleeAttackState) || _lastState.GetType() == typeof(RangedAttackState)) UseAttackDirection();


        }

        private void UseMovementDirection()
        {
            FacingDirection playerMoveDirection = player.PlayerMovement.CurrentPlayerMoveDirection;
            switch (playerMoveDirection)
            {
                case FacingDirection.Up: animator.CrossFade(IdleUpHash, crossFadeDuration); break;
                case FacingDirection.Down: animator.CrossFade(IdleDownHash, crossFadeDuration); break;
                case FacingDirection.Left: animator.CrossFade(IdleLeftHash, crossFadeDuration); break;
                case FacingDirection.Right: animator.CrossFade(IdleRightHash, crossFadeDuration); break;
            }
        }

        private void UseAttackDirection()
        {
            FacingDirection playerAttackDirection = player.PlayerAttackController.CurrentPlayerAttackDirection;
            switch (playerAttackDirection)
            {
                case FacingDirection.Up: animator.CrossFade(IdleUpHash, crossFadeDuration); break;
                case FacingDirection.Down: animator.CrossFade(IdleDownHash, crossFadeDuration); break;
                case FacingDirection.Left: animator.CrossFade(IdleLeftHash, crossFadeDuration); break;
                case FacingDirection.Right: animator.CrossFade(IdleRightHash, crossFadeDuration); break;
            }
        }
    }
}