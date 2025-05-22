using Terra.AI.Enemy;
using Terra.Enums;
using Terra.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates {  
    public class EnemyRangeAttackState : EnemyBaseAttackState {
        private const float AttackCooldownTime = 1f;  // Attack cooldown time (constant)
        private float _lastAttackTime;
        
        // Constructor to initialize the state
        public EnemyRangeAttackState(EnemyBase enemy, NavMeshAgent agent, Animator animator, PlayerEntity player) :
            base(enemy, agent, animator, player)
        {
            _lastAttackTime = Time.time;  // Initialize last attack time to current time
        }

        // Called when entering the attack state
        public override void OnEnter() {
            base.OnEnter();
            
            // Set the animation based on the direction the enemy is facing
            string animationName = enemy.CurrentDirection == FacingDirection.Left ? "RangeAttackLeft" : "RangeAttackRight";
            animator.CrossFade(animationName, CrossFadeDuration);  // Cross-fade to the attack animation
        }

        // The attack logic, called during each update
        protected override void OnAttack() {
            if (Player == null) {
                Debug.LogError($"{this}: Player is null");  // Check if the player exists
                return;
            }
            
            // Check if the player is within attack range
            if (!IsPlayerInRange()) {
                return;  // Exit if the player is out of range
            }

            // Check if the path to the player is clear (no obstacles)
            if (Time.time - _lastAttackTime >= AttackCooldownTime && IsPathClearToPlayer()) {
                // If there's no obstacle, perform the attack
                enemy.AttemptAttack();
                _lastAttackTime = Time.time;  // Update the last attack time
            } else {
                Debug.Log("Path is blocked or cooldown not finished, cannot attack.");  // Log if the path is blocked or cooldown is active
            }
        }

        // Check if the player is within the attack range
        private bool IsPlayerInRange() {
            float attackRange = 10f;  // Example attack range (can be adjusted)
            float distanceToPlayer = Vector3.Distance(enemy.transform.position, Player.transform.position);
            
            return distanceToPlayer <= attackRange;  // Return true if the player is within range
        }

        // Check if there is an unobstructed path to the player
        private bool IsPathClearToPlayer() {
            // Calculate direction towards the player
            Vector3 directionToPlayer = (Player.transform.position - enemy.transform.position).normalized;
            
            
            // Perform a raycast to check if anything blocks the path
            if (Physics.Raycast(enemy.transform.position, directionToPlayer, out RaycastHit hit, Mathf.Infinity)) {
                // If the ray hits something that isn't the player, return false (path is blocked)
                if (hit.transform != Player.transform) {
                    return false;
                }
            }
            return true;  // Return true if the path is clear
        }

        // Called when exiting the attack state
        public override void OnExit() {
            base.OnExit();
            navMeshAgent.isStopped = false;  // Resume the agent's movement when exiting the attack state
        }
    }
}
