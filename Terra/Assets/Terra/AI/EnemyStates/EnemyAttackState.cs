using Terra.AI.Enemy;
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

            navMeshAgent.velocity *= 1.5f;
            int animationName = enemy.CurrentDirection == FacingDirection.Left ? AnimationHashes.AttackLeft : AnimationHashes.AttackRight;
            animator.CrossFade(animationName, CrossFadeDuration);
        }

        protected override void OnAttack()
        {
            return;
        }
    }
}