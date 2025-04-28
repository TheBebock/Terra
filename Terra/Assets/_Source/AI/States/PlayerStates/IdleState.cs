using Terra.Player;
using UnityEngine;
using static Terra.Player.PlayerAttackController;
using static Terra.Player.PlayerMovement;

namespace Terra.StateMachine.PlayerStates
{
    public class IdleState : PlayerBaseState
    {
        private IState lastState;
        public IdleState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            lastState = player.GetLastState();
            ChangeDirectionOfAnimation();
        }

        private void ChangeDirectionOfAnimation()
        {
            if (lastState == null)
            {
                animator.CrossFade(IdleDownHash, CrossFadeDuration);
                return;
            }

            if (lastState.GetType() == typeof(LocomotionState)) UseMovementDirection();
            if (lastState.GetType() == typeof(MeleeAttackState) || lastState.GetType() == typeof(RangedAttackState)) UseAttackDirection();


        }

        private void UseMovementDirection()
        {
            PlayerMoveDirection playerMoveDirection = player.PlayerMovement.CurrentPlayerMoveDirection;
            switch (playerMoveDirection)
            {
                case PlayerMoveDirection.Up: animator.CrossFade(IdleUpHash, CrossFadeDuration); break;
                case PlayerMoveDirection.Down: animator.CrossFade(IdleDownHash, CrossFadeDuration); break;
                case PlayerMoveDirection.Left: animator.CrossFade(IdleLeftHash, CrossFadeDuration); break;
                case PlayerMoveDirection.Right: animator.CrossFade(IdleRightHash, CrossFadeDuration); break;
            }
        }

        private void UseAttackDirection()
        {
            PlayerAttackDirection playerAttackDirection = player.PlayerAttackController.CurrentPlayerAttackDirection;
            switch (playerAttackDirection)
            {
                case PlayerAttackDirection.Up: animator.CrossFade(IdleUpHash, CrossFadeDuration); break;
                case PlayerAttackDirection.Down: animator.CrossFade(IdleDownHash, CrossFadeDuration); break;
                case PlayerAttackDirection.Left: animator.CrossFade(IdleLeftHash, CrossFadeDuration); break;
                case PlayerAttackDirection.Right: animator.CrossFade(IdleRightHash, CrossFadeDuration); break;
            }
        }
    }
}