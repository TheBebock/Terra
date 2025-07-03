using Terra.AI.Enemy;
using Terra.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates.BossStates
{
    public class BossPostPumpAttack : BossBaseState
    {

        public BossPostPumpAttack(EnemyBase enemy, NavMeshAgent navMeshAgent, Animator animator) : base(enemy, navMeshAgent, animator)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            int animationName = enemy.CurrentDirection == FacingDirection.Left ? postPumpAttackLeft : postPumpAttackRight;
            animator.CrossFade(animationName, CrossFadeDuration);
        }
    }
}
