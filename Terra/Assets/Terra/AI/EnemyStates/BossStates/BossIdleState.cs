using Terra.AI.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates.BossStates
{
    public class BossIdleState : BossBaseState
    {
        public BossIdleState(EnemyBase enemy, NavMeshAgent navMeshAgent, Animator animator) : base(enemy, navMeshAgent, animator)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            animator.CrossFade(AnimationHashes.IdleLeft, CrossFadeDuration);
        }
    }
}
