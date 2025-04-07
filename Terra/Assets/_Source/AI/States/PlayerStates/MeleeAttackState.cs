using Terra.Player;
using UnityEngine;

namespace Terra.StateMachine.PlayerStates
{
    public class MeleeAttackState : PlayerBaseState
    {
        public MeleeAttackState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            //animator.CrossFade(MeleeAttackHash, CrossFadeDuration);

            
            //player.PlayerInventory.GetMeleeWeapon.PerformAttack(player.CurrentPosition, player.transform.rotation);
        }

        public override void Update()
        {
            if(animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                player.PlayerAttackController.IsTryingPerformMeleeAttack = false;
            }
        }
    }
}
