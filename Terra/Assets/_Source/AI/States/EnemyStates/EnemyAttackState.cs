using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.States.EnemyStates {
    public class EnemyAttackState : EnemyBaseState {
        readonly NavMeshAgent agent;
        readonly Transform player;
        
        public EnemyAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator) {
            this.agent = agent;
            this.player = player;
        }
        
        public override void OnEnter() {
            Debug.Log("Attack");

            string animationName = enemy.CurrentDirection == Enemy.FacingDirection.Left ? "AttackLeft" : "AttackRight";
            animator.CrossFade(animationName, crossFadeDuration);
        }

        
        public override void Update() {
            agent.SetDestination(player.position);
            enemy.AttemptAttack();
        }
    }
}