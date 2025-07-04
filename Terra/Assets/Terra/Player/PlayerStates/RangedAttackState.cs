using Terra.Enums;
using UnityEngine;

namespace Terra.Player.PlayerStates
{
    public class RangedAttackState : PlayerBaseState
    {
        private static readonly int RangeCooldown = Animator.StringToHash("RangeCooldown");

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
            float computedModifier = Mathf.Lerp(0.5f, 2f, animationSpeedModifier);
            animator.SetFloat(RangeCooldown, computedModifier);
        }

        private void ChangeDirectionOfAnimation(FacingDirection playerAttackDirection)
        {
            switch (playerAttackDirection)
            {
                case FacingDirection.Up: animator.CrossFade(RangedAttackUpHash, crossFadeDuration);
                    break;
                case FacingDirection.Down: animator.CrossFade(RangedAttackDownHash, crossFadeDuration);
                    break;
                case FacingDirection.Left: animator.CrossFade(RangedAttackLeftHash, crossFadeDuration);
                    break;
                case FacingDirection.Right: animator.CrossFade(RangedAttackRightHash, crossFadeDuration);
                    break;
            }
        }
    }
}
