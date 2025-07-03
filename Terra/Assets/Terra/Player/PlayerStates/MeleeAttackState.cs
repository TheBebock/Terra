using System.Collections.Generic;
using System.Linq;
using Terra.AnimationEvent;
using Terra.Enums;
using UnityEngine;

namespace Terra.Player.PlayerStates
{
    public class MeleeAttackState : PlayerBaseState
    {
        private int _actualStateHash;
        List<AnimationEventStateBehaviour> _animationEventStateBehaviours = new List<AnimationEventStateBehaviour>();
        public MeleeAttackState(PlayerManager player, Animator animator) : base(player, animator)
        
        {
            _animationEventStateBehaviours = animator.GetBehaviours<AnimationEventStateBehaviour>().ToList();

        }

        public override void OnEnter()
        {
            foreach (var b in _animationEventStateBehaviours)
            {
                b.ResetState();
            }
            ChangeDirectionOfAnimation(player.PlayerAttackController.CurrentPlayerAttackDirection); 
        }
        

        private void ChangeDirectionOfAnimation(FacingDirection playerAttackDirection)
        {
            switch (playerAttackDirection)
            {
                case FacingDirection.Up: animator.CrossFade(MeleeAttackUpHash, crossFadeDuration); _actualStateHash = MeleeAttackUpHash; break;
                case FacingDirection.Down: animator.CrossFade(MeleeAttackDownHash, crossFadeDuration); _actualStateHash = MeleeAttackDownHash; break;
                case FacingDirection.Left: animator.CrossFade(MeleeAttackLeftHash, crossFadeDuration); _actualStateHash = MeleeAttackLeftHash; break;
                case FacingDirection.Right: animator.CrossFade(MeleeAttackRightHash, crossFadeDuration); _actualStateHash = MeleeAttackRightHash; break;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            
            foreach (var b in _animationEventStateBehaviours)
            {
                b.ResetState();
            }
        }
    }
}
