using Terra.Enums;
using UnityEngine;

namespace Terra.Player.PlayerStates
{
    public class MeleeAttackState : PlayerBaseState
    {
        private int _actualStateHash;
        public MeleeAttackState(PlayerManager player, Animator animator) : base(player, animator)
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
            if(animator.GetCurrentAnimatorStateInfo(0).length < animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                player.PlayerAttackController.IsTryingPerformMeleeAttack = false;
            }
        }

        private void ChangeDirectionOfAnimation(FacingDirection playerAttackDirection)
        {
            switch (playerAttackDirection)
            {
                case FacingDirection.Up: animator.CrossFade(MeleeAttackUpHash, CrossFadeDuration); _actualStateHash = MeleeAttackUpHash; break;
                case FacingDirection.Down: animator.CrossFade(MeleeAttackDownHash, CrossFadeDuration); _actualStateHash = MeleeAttackDownHash; break;
                case FacingDirection.Left: animator.CrossFade(MeleeAttackLeftHash, CrossFadeDuration); _actualStateHash = MeleeAttackLeftHash; break;
                case FacingDirection.Right: animator.CrossFade(MeleeAttackRightHash, CrossFadeDuration); _actualStateHash = MeleeAttackRightHash; break;
            }
        }
    }
}
