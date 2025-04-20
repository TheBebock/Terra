using Terra.Player;
using UnityEngine;
using static Terra.Player.PlayerAttackController;

namespace Terra.StateMachine.PlayerStates
{
    public class MeleeAttackState : PlayerBaseState
    {
        public MeleeAttackState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            ChangeDirectionOfAnimation(player.PlayerAttackController.CurrentPlayerAttackDirection); 
            player.PlayerInventory.MeleeWeapon.PerformAttack(player.CurrentPosition, player.transform.rotation);
        }

        public override void Update()
        {
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
                case PlayerAttackDirection.Up: animator.CrossFade(MeleeAttackUpHash, CrossFadeDuration); break;
                case PlayerAttackDirection.Down: animator.CrossFade(MeleeAttackDownHash, CrossFadeDuration); break;
                case PlayerAttackDirection.Left: animator.CrossFade(MeleeAttackLeftHash, CrossFadeDuration); break;
                case PlayerAttackDirection.Right: animator.CrossFade(MeleeAttackRightHash, CrossFadeDuration); break;
            }
        }
    }
}
