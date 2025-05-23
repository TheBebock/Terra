using Terra.AI.Enemy;
using Terra.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates
{
    public class EnemyDeathState : EnemyBaseState
    {
        
        private bool _hasPlayedDeathAnimation;
        public EnemyDeathState(EnemyBase enemy, NavMeshAgent agent, Animator animator) : base(enemy, agent, animator)
        {
        }
        
        public override void OnEnter()
        {
            base.OnEnter();

            Debug.Log($"{enemy.name} Death state");
            animator.SetTrigger(AnimationHashes.Death);
            
            int animationName = enemy.CurrentDirection == FacingDirection.Left ? AnimationHashes.DeathLeft : AnimationHashes.DeathRight;
            animator.CrossFade(animationName, CrossFadeDuration);
        }
        
    }
}
