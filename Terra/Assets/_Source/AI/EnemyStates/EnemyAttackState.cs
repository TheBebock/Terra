using Terra.AI.Enemies;
using Terra.Enums;
using Terra.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates {
    public class EnemyAttackState : EnemyBaseAttackState {
        public EnemyAttackState(EnemyBase enemy, NavMeshAgent agent, Animator animator, PlayerEntity player) : base(enemy, agent, animator, player)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            string animationName = enemy.CurrentDirection == FacingDirection.Left ? "AttackLeft" : "AttackRight";
            animator.CrossFade(animationName, crossFadeDuration);
        }
        
        protected override void OnAttack()
        {
            enemy.AttemptAttack();
        }

        public override void OnExit()
        {
            base.OnExit();
            navMeshAgent.isStopped = false;
        }
    }
}