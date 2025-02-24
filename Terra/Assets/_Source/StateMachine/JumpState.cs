using UnityEngine;

namespace _Source.StateMachine
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            Animator.CrossFade(JumpHash,CrossFadeDuration);
        }

        public override void FixedUpdate()
        {
            //call Player's jump target and move logic.
        }
    }
}