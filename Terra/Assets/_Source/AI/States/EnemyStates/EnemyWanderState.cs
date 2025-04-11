using UnityEngine;
using UnityEngine.AI;

namespace Platformer {
    public class EnemyWanderState : EnemyBaseState {
        
        readonly NavMeshAgent agent;
        readonly Vector3 startPoint;
        readonly float wanderRadius;

        public EnemyWanderState(Enemy enemy, Animator animator, NavMeshAgent agent, float wanderRadius) : base(enemy, animator) {
            if (enemy == null || animator == null || agent == null) {
                Debug.LogError("Nieprawidłowe przypisanie komponentów! enemy, animator lub agent jest null.");
                return;
            }
            this.agent = agent;
            this.startPoint = enemy.transform.position;
            this.wanderRadius = wanderRadius;
        }
        
        public override void OnEnter() {
            Debug.Log("Wander");
    
            string animationName = enemy.CurrentDirection == Enemy.FacingDirection.Left ? "WalkLeft" : "WalkRight";
            animator.CrossFade(animationName, crossFadeDuration);
        }

        public override void Update() {
            if (agent == null) {
                Debug.LogError("NavMeshAgent jest null w EnemyWanderState.");
                return;
            }
            if (HasReachedDestination()) {
                var randomDirection = Random.insideUnitSphere * wanderRadius;
                randomDirection += startPoint;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
                var finalPosition = hit.position;
                
                agent.SetDestination(finalPosition);
            }
        }
        
        bool HasReachedDestination() {
            return !agent.pathPending
                   && agent.remainingDistance <= agent.stoppingDistance
                   && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
        }
    }
}