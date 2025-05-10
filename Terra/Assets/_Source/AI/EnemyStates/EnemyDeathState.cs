using Terra.AI.Enemies;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates
{
    public class EnemyDeathState : EnemyBaseState
    {
        public EnemyDeathState(EnemyBase enemy, NavMeshAgent agent, Animator animator) : base(enemy, agent, animator)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.CrossFade(DieHash, crossFadeDuration);
        }
    }
}