using UnityEngine;

namespace Terra.Player.PlayerStates
{
    public class DeathState : PlayerBaseState
    {
        public DeathState(PlayerManager player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.CrossFade(DeathHash, crossFadeDuration);
            Debug.Log($"{this}: death state");
        }
    }
} 