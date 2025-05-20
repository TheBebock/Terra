using Terra.AI.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates {
    public class EnemyChaseState : EnemyBaseState {

        readonly Transform _player;
        
        public EnemyChaseState(EnemyBase enemy, NavMeshAgent agent, Animator animator, Transform player) : base(enemy, agent, animator)
        {
            _player = player;
        }

        public override void OnEnter() {
            Debug.Log("Chase");

            Enemy.UpdateFacingDirection(_player);

            int animationHash = EnemyAnimHashes.WalkLeft;

            Animator.CrossFade(animationHash, CrossFadeDuration);
        }
        
        public override void Update() {

            if (!IsPlayerAlive) {

                return; // Do not chase further if the player is not alive
            }

            // Set the destination to the player's current position
            NavMeshAgent.SetDestination(_player.position);

            // Update the facing direction using the player's transform
            Enemy.UpdateFacingDirection(_player);
        }
    }
}