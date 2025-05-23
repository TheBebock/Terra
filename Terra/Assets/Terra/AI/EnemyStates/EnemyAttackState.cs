using Terra.AI.Enemy;
using Terra.Enums;
using Terra.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates {
    public class EnemyAttackState : EnemyBaseAttackState {
        
        private const float AttackCooldown = 1f;
        private float _lastAttackTime;
        public EnemyAttackState(EnemyBase enemy, NavMeshAgent agent, Animator animator, PlayerEntity player) : base(enemy, agent, animator, player)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            string animationName = enemy.CurrentDirection == FacingDirection.Left ? "AttackLeft" : "AttackRight";
            animator.CrossFade(animationName, CrossFadeDuration);
        }
        
        protected override void OnAttack()
        {
            
            if (Time.time - _lastAttackTime >= AttackCooldown)
            {
                if (Vector3.Distance(enemy.transform.position, Player.transform.position) <= enemy.AttackRange)
                {
                    enemy.AttemptAttack();
                }
                _lastAttackTime = Time.time;
            }
        }
    }
}