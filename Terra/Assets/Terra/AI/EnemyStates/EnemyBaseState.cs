using Terra.AI.Enemy;
using Terra.FSM;
using Terra.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates {
    /// <summary>
    /// Base class for all enemy states implementing IState.
    /// Provides shared references and utility logic.
    /// </summary>
    public abstract class EnemyBaseState : BaseState {
        /// <summary>
        /// Reference to the enemy entity using this state.
        /// </summary>
        protected readonly EnemyBase enemy;

        /// <summary>
        /// Reference to the enemy's Animator component.
        /// </summary>
        protected readonly Animator animator;

        /// <summary>
        /// Reference to the enemy's NavMeshAgent used for navigation.
        /// </summary>
        protected readonly NavMeshAgent navMeshAgent;

        /// <summary>
        /// Duration of cross-fade animations (in seconds).
        /// </summary>
        protected const float CrossFadeDuration = 0.1f;

        /// <summary>
        /// Constructs a new enemy state with required references.
        /// </summary>
        /// <param name="enemy">The enemy entity.</param>
        /// <param name="navMeshAgent">The NavMeshAgent used for movement.</param>
        /// <param name="animator">The Animator used for transitions.</param>
        protected EnemyBaseState(EnemyBase enemy, NavMeshAgent navMeshAgent, Animator animator) {
            this.enemy = enemy;
            this.navMeshAgent = navMeshAgent;
            this.animator = animator;
        }

        /// <summary>
        /// Indicates whether the player is currently alive based on health.
        /// Null-checked for safety.
        /// </summary>
        protected bool IsPlayerAlive =>
            PlayerManager.Instance != null &&
            PlayerManager.Instance.HealthController != null &&
            PlayerManager.Instance.HealthController.CurrentHealth > 0;
        
    }
}
