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
            
            string animationName = Enemy.CurrentDirection == FacingDirection.Left ? "AttackLeft" : "AttackRight";
            Animator.CrossFade(animationName, CrossFadeDuration);
        }
        
        protected override void OnAttack()
        {
            if (Time.time - _lastAttackTime >= AttackCooldown)
            {
                if (Vector3.Distance(Enemy.transform.position, Player.transform.position) <= Enemy.AttackRange)
                {
                    Enemy.AttemptAttack();
                }
                _lastAttackTime = Time.time;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            NavMeshAgent.isStopped = false;
        }
    }
}