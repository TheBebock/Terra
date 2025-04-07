using Terra.Player;
using UnityEngine;

namespace Terra.StateMachine.PlayerStates
{
    public class DeathState : PlayerBaseState
    {
        public DeathState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.CrossFade(DeathHash, CrossFadeDuration);
            player.OnDeath();
        }
    }
}