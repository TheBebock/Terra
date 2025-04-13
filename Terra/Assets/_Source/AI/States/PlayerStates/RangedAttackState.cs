using Terra.Player;
using UnityEngine;
using static Terra.Player.PlayerAttackController;

namespace Terra.StateMachine.PlayerStates
{
    public class RangedAttackState : PlayerBaseState
    {
        public RangedAttackState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            ChangeDirectionOfAnimation(player.PlayerAttackController.CurrentPlayerAttackDirection);

            player.PlayerInventory.GetRangedWeapon.PerformAttack(player.CurrentPosition, player.transform.rotation);
        }
        public override void Update()
        {
            // Disable player attack trigger when animation end
            if (animator.GetCurrentAnimatorStateInfo(0).length < animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                player.PlayerAttackController.IsTryingPerformDistanceAttack = false;
            }
        }

        private void ChangeDirectionOfAnimation(PlayerAttackDirection playerAttackDirection)
        {
            switch (playerAttackDirection)
            {
                case PlayerAttackDirection.Up: animator.CrossFade(RangedAttackUpHash, CrossFadeDuration); break;
                case PlayerAttackDirection.Down: animator.CrossFade(RangedAttackDownHash, CrossFadeDuration); break;
                case PlayerAttackDirection.Left: animator.CrossFade(RangedAttackLeftHash, CrossFadeDuration); break;
                case PlayerAttackDirection.Right: animator.CrossFade(RangedAttackRightHash, CrossFadeDuration); break;
            }
        }
    }
}
