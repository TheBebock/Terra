using Terra.AI.Enemy;
using Terra.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates
{
    public class EnemyTankTiredState : EnemyBaseState
    {
        public EnemyTankTiredState(EnemyBase enemy, NavMeshAgent navMeshAgent, Animator animator) : base(enemy, navMeshAgent, animator)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (navMeshAgent)
            {
                navMeshAgent.isStopped = true;
                navMeshAgent.velocity = Vector3.zero;
            }
           
            
            int animationName = enemy.CurrentDirection == FacingDirection.Left ? AnimationHashes.IdleLeft : AnimationHashes.IdleRight;
            animator.CrossFade(animationName, CrossFadeDuration);
        }

        public override void OnExit()
        {
            base.OnExit();
            if(navMeshAgent) navMeshAgent.isStopped = false;
        }
    }
}
