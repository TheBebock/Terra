using System.Collections.Generic;
using System.Linq;
using Terra.AnimationEvent;
using Terra.Enums;
using UnityEngine;

namespace Terra.Player.PlayerStates
{
    public class MeleeAttackState : PlayerBaseState
    {
        List<AnimationEventStateBehaviour> _animationEventStateBehaviours = new List<AnimationEventStateBehaviour>();
        private static readonly int SwingSpeed = Animator.StringToHash("SwingSpeed");
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
            ApplyAnimationModifiers();
            ChangeDirectionOfAnimation(player.PlayerAttackController.CurrentPlayerAttackDirection); 
        }

        private void ApplyAnimationModifiers()
        {
            float animationSpeedModifier = Mathf.InverseLerp(-100f, 100f, PlayerStatsManager.Instance.PlayerStats.SwingSpeed);
            animator.SetFloat(SwingSpeed, Mathf.Lerp(0.67f, 1.25f, animationSpeedModifier));
        }
        

        private void ChangeDirectionOfAnimation(FacingDirection playerAttackDirection)
        {
            switch (playerAttackDirection)
            {
                case FacingDirection.Up: animator.CrossFade(MeleeAttackUpHash, crossFadeDuration);
                    break;
                case FacingDirection.Down: animator.CrossFade(MeleeAttackDownHash, crossFadeDuration);
                    break;
                case FacingDirection.Left: animator.CrossFade(MeleeAttackLeftHash, crossFadeDuration);
                    break;
                case FacingDirection.Right: animator.CrossFade(MeleeAttackRightHash, crossFadeDuration);
                    break;
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
