using Terra.AI.Enemy;
using Terra.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates {
    public class EnemyChaseState : EnemyBaseState {

        readonly Transform _player;
        private int _animationName;
        public EnemyChaseState(EnemyBase enemy, NavMeshAgent agent, Animator animator, Transform player) : base(enemy, agent, animator)
        {
            _player = player;
        }

        public override void OnEnter()
        {
            Debug.Log("Chase");
    
            enemy.UpdateFacingDirection(enemy.GetNormalisedDirectionToPlayer());
            _animationName = enemy.CurrentDirection == FacingDirection.Left ? AnimationHashes.WalkLeft : AnimationHashes.WalkRight;
            animator.CrossFade(_animationName, CrossFadeDuration);
        }
        
        public override void Update() {

            if (!IsPlayerAlive || enemy.HealthController.IsDead) {

                return; // Do not chase further if the player is not alive
            }

            // Set the destination to the player's current position
            if(navMeshAgent.enabled && !navMeshAgent.isStopped) navMeshAgent.SetDestination(_player.position);
            
            int temp = enemy.CurrentDirection == FacingDirection.Left ? AnimationHashes.WalkLeft : AnimationHashes.WalkRight;
            if(temp == _animationName) return;
            _animationName = temp;
            animator.CrossFade(_animationName, CrossFadeDuration);
        }
    }
}