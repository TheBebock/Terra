using Terra.Enums;
using UnityEngine;

namespace Terra.Player.PlayerStates
{
    public class RangedAttackState : PlayerBaseState
    {
        private int _actualStateHash;
        public RangedAttackState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            ChangeDirectionOfAnimation(player.PlayerAttackController.CurrentPlayerAttackDirection);
        }
        public override void Update()
        {
            // Check if animator already changed state
            if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != _actualStateHash) return;

            // Disable player attack trigger when animation end
            if (animator.GetCurrentAnimatorStateInfo(0).length < animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                player.PlayerAttackController.IsTryingPerformDistanceAttack = false;
            }
        }

        private void ChangeDirectionOfAnimation(FacingDirection playerAttackDirection)
        {
            switch (playerAttackDirection)
            {
                case FacingDirection.Up: animator.CrossFade(RangedAttackUpHash, CrossFadeDuration); _actualStateHash = RangedAttackUpHash; break;
                case FacingDirection.Down: animator.CrossFade(RangedAttackDownHash, CrossFadeDuration); _actualStateHash = RangedAttackDownHash; break;
                case FacingDirection.Left: animator.CrossFade(RangedAttackLeftHash, CrossFadeDuration); _actualStateHash = RangedAttackLeftHash; break;
                case FacingDirection.Right: animator.CrossFade(RangedAttackRightHash, CrossFadeDuration); _actualStateHash = RangedAttackRightHash; break;
            }
        }
    }
}
