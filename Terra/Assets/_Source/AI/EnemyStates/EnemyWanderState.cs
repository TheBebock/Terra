using Terra.AI.Enemy;
using Terra.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates {

    // State representing the enemy wandering behavior
    public class EnemyWanderState : EnemyBaseState {
        
        // The initial position of the enemy when it starts wandering
        readonly Vector3 _startPoint;
        
        // The radius within which the enemy can wander
        readonly float _wanderRadius;

        // Animation hashes to avoid string lookup in every call
        private static readonly int WalkLeftHash = Animator.StringToHash("WalkLeft");
        private static readonly int WalkRightHash = Animator.StringToHash("WalkRight");

        // Constructor to initialize the enemy wander state
        public EnemyWanderState(EnemyBase enemy, NavMeshAgent navMeshAgent, Animator animator, float wanderRadius) : base(enemy, navMeshAgent, animator)
        {
            // Set the start point and wander radius based on the enemy's current position and the given radius
            this._startPoint = enemy.transform.position;
            this._wanderRadius = wanderRadius;
        }

        // Called when the enemy enters the wander state
        public override void OnEnter() 
        {
            Debug.Log("Wander");
    
            // Use appropriate animation based on the direction the enemy is facing
            int animationHash = Enemy.CurrentDirection == FacingDirection.Left ? WalkLeftHash : WalkRightHash;
            Animator.CrossFade(animationHash, CrossFadeDuration);
        }

        // Called every frame while the enemy is in the wander state
        public override void Update() 
        {
            if (NavMeshAgent == null) {
                Debug.LogError("NavMeshAgent is null in EnemyWanderState.");
                return;
            }

            // Check if the enemy has reached its current destination
            if (HasReachedDestination()) {
                // Randomly select a new direction within the wander radius
                Vector3 randomDirection = Random.insideUnitSphere * _wanderRadius + _startPoint;
                
                // Sample the NavMesh to find a valid position within the radius
                if (NavMesh.SamplePosition(randomDirection, out var hit, _wanderRadius, NavMesh.AllAreas)) {
                    // Set the new destination for the enemy
                    NavMeshAgent.SetDestination(hit.position);
                } else {
                    // Log a warning if a valid point was not found on the NavMesh
                    Debug.LogWarning("No valid point found on NavMesh.");
                }
            }
        }

        // Checks if the enemy has reached its destination
        bool HasReachedDestination() 
        {
            float tolerance = 0.1f;
            return !NavMeshAgent.pathPending
                   && NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance + tolerance
                   && (!NavMeshAgent.hasPath || NavMeshAgent.velocity.sqrMagnitude == 0f);
        }
    }
}
