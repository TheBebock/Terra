using Terra.Player;
using UnityEngine;

namespace Terra.StateMachine.PlayerStates
{
    public class RangedAttackState : PlayerBaseState
    {
        public RangedAttackState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            //animator.CrossFade(RangedAttackHash, CrossFadeDuration);


            //player.PlayerInventory.GetRangedWeapon.PerformAttack(player.CurrentPosition, player.transform.rotation);
        }
        public override void Update()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                player.PlayerAttackController.IsTryingPerformMeleeAttack = false;
            }
        }
    }
}
