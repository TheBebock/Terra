using Terra.AI.Enemy;
using Terra.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates
{
    /// <summary>
    /// Base class for all enemy attack states. Inherits from <see cref="EnemyBaseState"/>.
    /// Handles attack-related logic and interactions with the player.
    /// </summary>
    public abstract class EnemyBaseAttackState : EnemyBaseState
    {
        /// <summary>
        /// Reference to the player entity being attacked.
        /// </summary>
        protected readonly PlayerEntity player;

        /// <summary>
        /// Constructs a new attack state with required references.
        /// </summary>
        /// <param name="enemy">The enemy entity.</param>
        /// <param name="agent">The NavMeshAgent used for movement.</param>
        /// <param name="animator">The Animator used for animations.</param>
        /// <param name="player">The player entity that the enemy will attack.</param>
        protected  EnemyBaseAttackState(EnemyBase enemy, NavMeshAgent agent, Animator animator, PlayerEntity player) : base(
            enemy, agent, animator)
        {
            this.player = player;
        }

        /// <summary>
        /// Called when the attack state is entered. Stops the agent and freezes movement.
        /// </summary>
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log($"{enemy.name} Attack");
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }

        /// <summary>
        /// Called every frame while the attack state is active. Executes the attack logic.
        /// </summary>
        public override void Update()
        {
            base.Update();
            OnAttack();
        }

        /// <summary>
        /// Implements attack logic. Must be overridden by subclasses to define specific attack behavior.
        /// </summary>
        protected abstract void OnAttack();

        /// <summary>
        /// Called when the attack state is exited. Resumes movement by un-pausing the agent.
        /// </summary>
        public override void OnExit()
        {
            base.OnExit();
            if(navMeshAgent.isActiveAndEnabled) navMeshAgent.isStopped = false;
        }
    }
}
