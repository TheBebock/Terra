using Terra.AI.Enemy;
using Terra.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates.BossStates
{
    public class BossAcidSpitAttack : BossBaseState
    {
        public BossAcidSpitAttack(EnemyBase enemy, NavMeshAgent navMeshAgent, Animator animator) : base(enemy, navMeshAgent, animator)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
            boss.RangeAttackStart();
            int animHash = boss.CurrentDirection == FacingDirection.Left ? rangeAttackLeft : rangeAttackRight;
            animator.CrossFade(animHash, CrossFadeDuration);
        }

        public override void OnExit()
        {
            base.OnExit();
            navMeshAgent.isStopped = false;
        }
    }
}
