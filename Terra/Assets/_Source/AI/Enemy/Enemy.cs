using System.Collections.Generic;
using _Source.AI.Enemy;
using Core.ModifiableValue;
using StatisticsSystem.Definitions;
using Terra.StateMachine;
using Terra.AI.States.EnemyStates;
using Terra.Combat;
using Terra.Player;
using Terra.Utils;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Represents an enemy entity with AI-driven behavior, health management, and combat capabilities.
/// Initializes states for wandering, chasing, attacking, and death, and reacts to damage and death events.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PlayerDetector))]
public class Enemy : Entity, IInitializable, IDamagable
{
    // Animator parameter hash for controlling facing direction
    private static readonly int Direction = Animator.StringToHash("Direction");

    [Header("Components")]
    [SerializeField]
    private NavMeshAgent agent;               // Reference to Unity's NavMeshAgent for navigation
    [SerializeField]
    private PlayerDetector playerDetector;    // Component to detect player presence
    [SerializeField]
    private Animator animator;                // Animator controlling enemy animations

    [Header("Stats")]
    [SerializeField]
    private EnemyStatsDefinition enemyStats;  // ScriptableObject containing base stats (health, speed, etc.)
    private HealthController _healthController;// Handles health, damage, and death events
    public HealthController HealthController => _healthController;

    [Header("Combat")]
    [SerializeField]
    private float detectionRadius = 5f;       // Radius within which the enemy can detect the player
    [SerializeField]
    private float attackRadius = 1.5f;        // Radius within which the enemy can perform an attack
    [SerializeField]
    private float timeBetweenAttacks = 1f;    // Cooldown between consecutive attacks
    [SerializeField]
    private float attackDamage = 1f;          // Damage dealt per attack

    // Facing direction state (used for animation and orientation)
    public enum FacingDirection { Left, Right }
    public FacingDirection CurrentDirection { get; private set; } = FacingDirection.Right;

    // Utility properties to check if enemy can take damage
    public bool IsInvincible => _healthController.IsInvincible;
    public bool CanBeDamaged => !_healthController.IsInvincible && _healthController.CurrentHealth > 0f;

    private bool isDead = false;                // Tracks death state to prevent further updates
    private StateMachine stateMachine;          // Manages AI states and transitions
    private EnemyDeathState enemyDeathState;    // Dedicated state for death behavior
    private CountdownTimer attackTimer;         // Timer controlling attack cooldown

    public bool IsInitialized { get; set; } // Indicates whether Initialize() was called successfully

    /// <summary>
    /// Initializes the enemy by validating components, setting up health, timing, and AI state machine.
    /// Subscribes to death event and configures all state transitions.
    /// </summary>
    public void Initialize()
    {
        // Validate required components and stats
        if (agent == null || playerDetector == null || animator == null || enemyStats == null)
        {
            Debug.LogError("Enemy initialization failed: missing components or stats.");
            return;
        }

        // Instantiate health controller with base max health and subscribe to OnDeath event
        _healthController = new HealthController(new ModifiableValue(enemyStats.baseMaxHealth));
        _healthController.OnDeath += OnDeath;

        // Initialize attack cooldown timer
        attackTimer = new CountdownTimer(timeBetweenAttacks);

        // Construct AI state machine and define all states
        stateMachine = new StateMachine();
        var wanderState = new EnemyWanderState(this, animator, agent, detectionRadius);
        var chaseState = new EnemyChaseState(this, animator, agent, PlayerManager.Instance.transform);
        var attackState = new EnemyAttackState(this, animator, agent, PlayerManager.Instance.transform);
        enemyDeathState = new EnemyDeathState(this, animator);

        // Configure transitions between states
        Any(enemyDeathState,    new FuncPredicate(() => _healthController.IsDead));         // Global transition to death
        At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
        At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
        At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
        At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));

        // Set initial state and mark initialization complete
        stateMachine.SetState(wanderState);
        IsInitialized = true;
    }

    /// <summary>
    /// Helper to add a transition from one state to another based on a predicate.
    /// </summary>
    private void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);

    /// <summary>
    /// Helper to add a global transition to a state based on a predicate.
    /// </summary>
    private void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    /// <summary>
    /// Main update loop. Updates AI, attack timer, and facing direction if alive.
    /// </summary>
    private void Update()
    {
        if (isDead || !IsInitialized)
            return;

        stateMachine.Update();
        attackTimer.Tick(Time.deltaTime);
        UpdateFacingDirection();
    }

    /// <summary>
    /// Fixed update loop for physics-based state logic.
    /// </summary>
    private void FixedUpdate()
    {
        if (isDead || !IsInitialized)
            return;

        stateMachine.FixedUpdate();
    }

    /// <summary>
    /// Performs an attack by dealing damage to all IDamagable targets within attack radius.
    /// </summary>
    public void AttemptAttack()
    {
        if (!attackTimer.IsFinished)
            return;

        var targets = ContactProvider.GetTargetsInSphere<IDamagable>(
            transform.position, attackRadius, ContactProvider.EnemyTargetsMask);

        foreach (var target in targets)
        {
            target.TakeDamage(attackDamage);
        }

        attackTimer.Reset();
    }

    /// <summary>
    /// Applies incoming damage via HealthController and spawns a damage popup.
    /// </summary>
    public void TakeDamage(float amount)
    {
        if (!CanBeDamaged) 
            return;

        _healthController.TakeDamage(amount);
        PopupDamageManager.Instance.CreatePopup(transform.position, Quaternion.identity, amount);
    }

    /// <summary>
    /// Adjusts the facing direction based on horizontal velocity and updates animator parameter.
    /// </summary>
    private void UpdateFacingDirection()
    {
        if (agent.velocity.x > 0.05f)
        {
            CurrentDirection = FacingDirection.Right;
            animator.SetInteger(Direction, 1);
        }
        else if (agent.velocity.x < -0.05f)
        {
            CurrentDirection = FacingDirection.Left;
            animator.SetInteger(Direction, 0);
        }
    }

    /// <summary>
    /// Handles death: stops AI and navigation, plays death animation, switches to death state, and destroys object.
    /// </summary>
    public void OnDeath()
    {
        if (isDead) 
            return;

        isDead = true;

        animator.SetTrigger("Die");
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.updatePosition = false;
        agent.updateRotation = false;

        stateMachine.SetState(enemyDeathState);
        Destroy(gameObject, 5f);
    }
}
