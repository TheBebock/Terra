using Terra.AI.Enemy;
using Terra.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates
{
    public class EnemyTankTiredState : EnemyBaseState
    {
        private EnemyTank _tank;
        public EnemyTankTiredState(EnemyBase enemy, NavMeshAgent navMeshAgent, Animator animator) : base(enemy, navMeshAgent, animator)
        {
            if (enemy is EnemyTank tank)
            {
                _tank = tank;
            }
            else
            {
                Debug.LogError($"{enemy.name} is not a {nameof(EnemyTank)}, but has {nameof(EnemyTankTiredState)} assigned");
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            int animationName = enemy.CurrentDirection == FacingDirection.Left ? AnimationHashes.IdleLeft : AnimationHashes.IdleRight;
            animator.CrossFade(animationName, CrossFadeDuration);
        }
    }
}
