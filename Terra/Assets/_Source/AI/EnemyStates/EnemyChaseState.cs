using Terra.AI.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates {
    public class EnemyChaseState : EnemyBaseState {

        readonly Transform _player;
        
        public EnemyChaseState(EnemyBase enemy, NavMeshAgent agent, Animator animator, Transform player) : base(enemy, agent, animator)
        {
            this._player = player;
        }

        public override void OnEnter() {
            Debug.Log("Chase");

            // Update the facing direction when chasing the player
            Enemy.UpdateFacingDirection(_player);

            // Use predefined animation hashes for better performance and consistency
            int animationHash = EnemyAnimHashes.Walk; // Use walk animation for right-facing enemy

            // Trigger the animation with smooth cross-fade using the animation hash
            Animator.CrossFade(animationHash, CrossFadeDuration);
        }
        
        public override void Update() {
            // If the player is dead, stop chasing and change state to something else
            if (!IsPlayerAlive) {
                // Transition to another state like wandering or idle
                return; // Do not chase further if the player is not alive
            }

            // Set the destination to the player's current position
            NavMeshAgent.SetDestination(_player.position);

            // Update the facing direction using the player's transform
            Enemy.UpdateFacingDirection(_player);
        }
    }
}