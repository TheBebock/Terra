using Terra.AI.Enemies;
using Terra.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates {
    public class EnemyChaseState : EnemyBaseState {

        readonly Transform player;
        
        public EnemyChaseState(EnemyBase enemy, NavMeshAgent agent, Animator animator, Transform player) : base(enemy, agent, animator)
        {
            this.player = player;
        }

        public override void OnEnter() {
            Debug.Log("Chase");
    
            string animationName = enemy.CurrentDirection == FacingDirection.Left ? "WalkLeft" : "WalkRight";
            animator.CrossFade(animationName, crossFadeDuration);
        }
        
        public override void Update() 
        {
            navMeshAgent.SetDestination(player.position);
        }
    }
}