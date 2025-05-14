using _Source.AI.Enemy;
using Terra.AI.Enemies;
using Terra.FSM;
using Terra.Player;
using UnityEngine;
using UnityEngine.AI;

namespace _Source.AI.EnemyStates {
    /// <summary>
    /// Base class for all enemy states implementing IState.
    /// Provides shared references and utility logic.
    /// </summary>
    public abstract class EnemyBaseState : IState {
        /// <summary>
        /// Reference to the enemy entity using this state.
        /// </summary>
        protected readonly EnemyBase Enemy;

        /// <summary>
        /// Reference to the enemy's Animator component.
        /// </summary>
        protected readonly Animator Animator;

        /// <summary>
        /// Reference to the enemy's NavMeshAgent used for navigation.
        /// </summary>
        protected readonly NavMeshAgent NavMeshAgent;

        /// <summary>
        /// Duration of cross-fade animations (in seconds).
        /// </summary>
        protected const float CrossFadeDuration = 0.1f;

        /// <summary>
        /// Constructs a new enemy state with required references.
        /// </summary>
        /// <param name="enemy">The enemy entity.</param>
        /// <param name="agent">The NavMeshAgent used for movement.</param>
        /// <param name="animator">The Animator used for transitions.</param>
        protected EnemyBaseState(EnemyBase enemy, NavMeshAgent agent, Animator animator) {
            this.Enemy = enemy;
            this.NavMeshAgent = agent;
            this.Animator = animator;
        }

        /// <summary>
        /// Indicates whether the player is currently alive based on health.
        /// Null-checked for safety.
        /// </summary>
        protected bool IsPlayerAlive =>
            PlayerManager.Instance != null &&
            PlayerManager.Instance.HealthController != null &&
            PlayerManager.Instance.HealthController.CurrentHealth > 0;

        /// <summary>
        /// Called when the state is entered.
        /// Used to initialize the state.
        /// </summary>
        public virtual void OnEnter() { }

        /// <summary>
        /// Called every frame while the state is active.
        /// Used for logic updates.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Called when the state is exited.
        /// Used to clean up state-specific data.
        /// </summary>
        public virtual void OnExit() { }
    }
}
