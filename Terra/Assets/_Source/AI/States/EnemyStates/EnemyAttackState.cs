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
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            
            string animationName = enemy.CurrentDirection == Enemy.FacingDirection.Left ? "AttackLeft" : "AttackRight";
            animator.CrossFade(animationName, crossFadeDuration);
        }

        
        public override void Update() 
        {
            base.Update();    
            enemy.AttemptAttack();
        }

        public override void OnExit()
        {
            base.OnExit();
            agent.isStopped = false;
        }
    }
}