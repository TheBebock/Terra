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
            ApplyAnimationModifiers();
            ChangeDirectionOfAnimation(player.PlayerAttackController.CurrentPlayerAttackDirection);
        }

        private void ApplyAnimationModifiers()
        {
            float animationSpeedModifier = Mathf.InverseLerp(-100f, 100f, PlayerStatsManager.Instance.PlayerStats.RangeCooldown);
            animator.SetFloat("RangeCooldown", Mathf.Lerp(0.8f, 1.6f, animationSpeedModifier));
        }

        private void ChangeDirectionOfAnimation(FacingDirection playerAttackDirection)
        {
            switch (playerAttackDirection)
            {
                case FacingDirection.Up: animator.CrossFade(RangedAttackUpHash, crossFadeDuration); _actualStateHash = RangedAttackUpHash; break;
                case FacingDirection.Down: animator.CrossFade(RangedAttackDownHash, crossFadeDuration); _actualStateHash = RangedAttackDownHash; break;
                case FacingDirection.Left: animator.CrossFade(RangedAttackLeftHash, crossFadeDuration); _actualStateHash = RangedAttackLeftHash; break;
                case FacingDirection.Right: animator.CrossFade(RangedAttackRightHash, crossFadeDuration); _actualStateHash = RangedAttackRightHash; break;
            }
        }
    }
}
