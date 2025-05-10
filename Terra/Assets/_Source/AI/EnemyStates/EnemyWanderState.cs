using Terra.AI.Enemies;
using Terra.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates {
    
    public class EnemyWanderState : EnemyBaseState {
        
        readonly Vector3 startPoint;
        readonly float wanderRadius;

        public EnemyWanderState(EnemyBase enemy, NavMeshAgent navMeshAgent, Animator animator, float wanderRadius) : base(enemy, navMeshAgent, animator)
        {
            this.startPoint = enemy.transform.position;
            this.wanderRadius = wanderRadius;
        }

        public override void OnEnter() 
        {
            Debug.Log("Wander");
    
            string animationName = enemy.CurrentDirection == FacingDirection.Left ? "WalkLeft" : "WalkRight";
            animator.CrossFade(animationName, crossFadeDuration);
        }

        public override void Update() 
        {
            if (navMeshAgent == null) {
                Debug.LogError("NavMeshAgent jest null w EnemyWanderState.");
                return;
            }
            
            if (HasReachedDestination()) {
                var randomDirection = Random.insideUnitSphere * wanderRadius;
                randomDirection += startPoint;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
                var finalPosition = hit.position;
                
                navMeshAgent.SetDestination(finalPosition);
            }
        }
        
        bool HasReachedDestination() 
        {
            return !navMeshAgent.pathPending
                   && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance
                   && (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f);
        }
    }
}