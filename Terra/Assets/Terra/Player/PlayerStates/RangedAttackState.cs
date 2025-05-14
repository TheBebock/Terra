using UnityEngine;
using static Terra.Player.PlayerAttackController;

namespace Terra.Player.PlayerStates
{
    public class RangedAttackState : PlayerBaseState
    {
        private int actualStateHash;
        public RangedAttackState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            ChangeDirectionOfAnimation(player.PlayerAttackController.CurrentPlayerAttackDirection);

            PlayerInventoryManager.Instance.RangedWeapon.PerformAttack(player.CurrentPosition, player.transform.rotation);
        }
        public override void Update()
        {
            // Check if animator already changed state
            if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != actualStateHash) return;

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
                case PlayerAttackDirection.Up: animator.CrossFade(RangedAttackUpHash, CrossFadeDuration); actualStateHash = RangedAttackUpHash; break;
                case PlayerAttackDirection.Down: animator.CrossFade(RangedAttackDownHash, CrossFadeDuration); actualStateHash = RangedAttackDownHash; break;
                case PlayerAttackDirection.Left: animator.CrossFade(RangedAttackLeftHash, CrossFadeDuration); actualStateHash = RangedAttackLeftHash; break;
                case PlayerAttackDirection.Right: animator.CrossFade(RangedAttackRightHash, CrossFadeDuration); actualStateHash = RangedAttackRightHash; break;
            }
        }
    }
}
