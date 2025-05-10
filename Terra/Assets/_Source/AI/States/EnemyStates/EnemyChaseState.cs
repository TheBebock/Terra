using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.States.EnemyStates {
    public class EnemyChaseState : EnemyBaseState {
        readonly NavMeshAgent agent;
        readonly Transform player;
        
        public EnemyChaseState(EnemyBase enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator) {
            this.agent = agent;
            this.player = player;
        }
        
        public override void OnEnter() {
            Debug.Log("Chase");
    
            string animationName = enemy.CurrentDirection == Enemy.FacingDirection.Left ? "WalkLeft" : "WalkRight";
            animator.CrossFade(animationName, crossFadeDuration);
        }
        
        public override void Update() 
        {
            
            agent.SetDestination(player.position);
        }
    }
}