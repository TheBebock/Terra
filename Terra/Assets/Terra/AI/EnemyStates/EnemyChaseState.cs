using Terra.AI.Enemy;
using Terra.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates {
    public class EnemyChaseState : EnemyBaseState {

        readonly Transform _player;
        
        public EnemyChaseState(EnemyBase enemy, NavMeshAgent agent, Animator animator, Transform player) : base(enemy, agent, animator)
        {
            _player = player;
        }

        public override void OnEnter()
        {
            Debug.Log("Chase");

            int animationName = enemy.CurrentDirection == FacingDirection.Left ? AnimationHashes.WalkLeft : AnimationHashes.WalkRight;
            animator.CrossFade(animationName, CrossFadeDuration);
        }
        
        public override void Update() {

            if (!IsPlayerAlive || enemy.HealthController.IsDead) {

                return; // Do not chase further if the player is not alive
            }

            // Set the destination to the player's current position
            if(navMeshAgent.enabled && !navMeshAgent.isStopped) navMeshAgent.SetDestination(_player.position);
        }
    }
}