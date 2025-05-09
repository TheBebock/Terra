using Terra.Player;
using UnityEngine;
using static Terra.Player.PlayerAttackController;

namespace Terra.StateMachine.PlayerStates
{
    public class MeleeAttackState : PlayerBaseState
    {
        private int actualStateHash;
        public MeleeAttackState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            ChangeDirectionOfAnimation(player.PlayerAttackController.CurrentPlayerAttackDirection); 
            PlayerInventoryManager.Instance.MeleeWeapon.PerformAttack(player.CurrentPosition, player.transform.rotation);
        }

        public override void Update()
        {
            // Check if animator already changed state
            if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != actualStateHash) return;

            // Disable player attack trigger when animation end
            if(animator.GetCurrentAnimatorStateInfo(0).length < animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                player.PlayerAttackController.IsTryingPerformMeleeAttack = false;
            }
        }

        private void ChangeDirectionOfAnimation(PlayerAttackDirection playerAttackDirection)
        {
            switch (playerAttackDirection)
            {
                case PlayerAttackDirection.Up: animator.CrossFade(MeleeAttackUpHash, CrossFadeDuration); actualStateHash = MeleeAttackUpHash; break;
                case PlayerAttackDirection.Down: animator.CrossFade(MeleeAttackDownHash, CrossFadeDuration); actualStateHash = MeleeAttackDownHash; break;
                case PlayerAttackDirection.Left: animator.CrossFade(MeleeAttackLeftHash, CrossFadeDuration); actualStateHash = MeleeAttackLeftHash; break;
                case PlayerAttackDirection.Right: animator.CrossFade(MeleeAttackRightHash, CrossFadeDuration); actualStateHash = MeleeAttackRightHash; break;
            }
        }
    }
}
