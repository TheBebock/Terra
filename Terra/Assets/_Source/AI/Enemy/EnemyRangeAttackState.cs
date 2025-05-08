using Terra.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.States.EnemyStates
{
    /// <summary>
    /// State representing a ranged enemy's attack behavior.
    /// </summary>
    public class EnemyRangeAttackState : IState
    {
        private readonly EnemyRange enemy;      // Reference to the ranged enemy
        private readonly Animator animator;     // Animator controlling the enemy's animations
        private readonly NavMeshAgent agent;    // NavMeshAgent used for movement
        private readonly Transform player;      // Reference to the player's transform

        /// <summary>
        /// Initializes the state with required components.
        /// </summary>
        public EnemyRangeAttackState(EnemyRange enemy, Animator animator, NavMeshAgent agent, Transform player)
        {
            this.enemy = enemy;
            this.animator = animator;
            this.agent = agent;
            this.player = player;
        }

        /// <summary>
        /// Called when the state is entered. Stops the enemy's movement.
        /// </summary>
        public void OnEnter()
        {
            agent.isStopped = true;
        }

        /// <summary>
        /// Not used in this state. Throws exception if called.
        /// </summary>
        public void Update()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Not used in this state. Throws exception if called.
        /// </summary>
        public void FixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called when the state is exited. Resumes the enemy's movement.
        /// </summary>
        public void OnExit()
        {
            agent.isStopped = false;
        }

        /// <summary>
        /// Called every frame while in this state.
        /// Handles enemy orientation and triggers ranged attack attempts.
        /// </summary>
        public void Tick()
        {
            if (player == null)
                return;

            // Rotate to face the player
            Vector3 direction = (player.position - enemy.transform.position).normalized;
            if (direction.x > 0)
                enemy.CurrentDirection = EnemyRange.FacingDirection.Right;
            else
                enemy.CurrentDirection = EnemyRange.FacingDirection.Left;

            // Attempt to perform a ranged attack
            enemy.AttemptAttack();
        }

        /// <summary>
        /// Not used in this state. Provided for interface compatibility.
        /// </summary>
        public void FixedTick() { }
    }
}
