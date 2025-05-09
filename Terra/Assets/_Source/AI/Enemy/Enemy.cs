using System;
using System.Collections.Generic;
using _Source.AI.Enemy;
using Core.ModifiableValue;
using DG.Tweening;
using StatisticsSystem.Definitions;
using Terra.StateMachine;
using Terra.AI.States.EnemyStates;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.Interfaces;
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
public class Enemy : Entity, IDamagable, IAttachListeners
{
    // Animator parameter hash for controlling facing direction
    private static readonly int Direction = Animator.StringToHash("Direction");

    [Header("Components")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayerDetector playerDetector;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider enemyCollider;
    [SerializeField] private SpriteRenderer enemyModel;
    
    [Header("Stats")]
    [SerializeField]
    private EnemyStatsDefinition enemyStats;  // ScriptableObject containing base stats (health, speed, etc.)
    private HealthController _healthController;// Handles health, damage, and death events

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
    public bool CanBeDamaged => _healthController.CurrentHealth > 0f;
    public HealthController HealthController => _healthController;

    
    private bool stateMachineUpdateLock = false;
    private bool isDead = false;                // Tracks death state to prevent further updates
    private StateMachine stateMachine;          // Manages AI states and transitions
    private EnemyDeathState enemyDeathState;    // Dedicated state for death behavior
    private CountdownTimer attackTimer;         // Timer controlling attack cooldown

    /// <summary>
    /// Initializes the enemy by validating components, setting up health, timing, and AI state machine.
    /// Subscribes to death event and configures all state transitions.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        
        // Validate required components and stats
        if (agent == null || playerDetector == null || animator == null || enemyStats == null)
        {
            Debug.LogError("Enemy initialization failed: missing components or stats.");
            return;
        }

        // Instantiate health controller with base max health and subscribe to OnDeath event
        _healthController = new HealthController(new ModifiableValue(enemyStats.baseMaxHealth));


        // Initialize attack cooldown timer
        attackTimer = new CountdownTimer(timeBetweenAttacks);

        // Construct AI state machine and define all states
        stateMachine = new StateMachine();
        var wanderState = new EnemyWanderState(this, animator, agent, detectionRadius);
        var chaseState = new EnemyChaseState(this, animator, agent, PlayerManager.Instance.transform);
        var attackState = new EnemyAttackState(this, animator, agent, PlayerManager.Instance.transform);
        enemyDeathState = new EnemyDeathState(this, animator);

        // Configure transitions between states
        At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
        At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
        At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
        At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));

        Any(enemyDeathState, new FuncPredicate(()=> isDead));
        // Set initial state and mark initialization complete
        stateMachine.SetState(wanderState);
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
        if (isDead || stateMachineUpdateLock)
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
        if (isDead || stateMachineUpdateLock)
            return;

        stateMachine.FixedUpdate();
    }

    /// <summary>
    /// Performs an attack by dealing damage to targeted IDamagable targets within attack radius.
    /// </summary>
    public void AttemptAttack()
    {
        if (!attackTimer.IsFinished)
            return;

        var targets = ComponentProvider.GetTargetsInSphere<IDamagable>(
            transform.position, attackRadius, ComponentProvider.EnemyTargetsMask);

        CombatManager.Instance.EnemyPerformedAttack(targets, enemyStats.baseStrength);
        
        attackTimer.Reset();
    }

    private void OnDamaged(float value)
    {
        enemyModel.material.DOColor(Color.red, 0.25f)
            .SetLoops(2, LoopType.Yoyo);
    }
    
    /// <summary>
    /// Applies incoming damage via HealthController and spawns a damage popup.
    /// </summary>
    public void TakeDamage(float amount, bool isPercentage = false)
    {
        if (!CanBeDamaged) 
            return;

        Debug.Log($"{gameObject.name} took {amount} damage");
        _healthController.TakeDamage(amount, isPercentage);
        PopupDamageManager.Instance.UsePopup(transform.position, Quaternion.identity, amount);
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


    public void CanUpdateState(bool value) => stateMachineUpdateLock = value;
    
    /// <summary>
    /// Handles death: stops AI and navigation, switches to death state, and destroys object.
    /// </summary>
    public void OnDeath()
    {
        if (isDead) 
            return;

        isDead = true;

        enemyCollider.enabled = false;
        
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.updatePosition = false;
        agent.updateRotation = false;
        
        //TODO: Remove and add normal death anim
        animator.enabled = false;

        enemyModel.material.DOFade(0f, 3.5f);
        
        stateMachine.SetState(enemyDeathState);
        Destroy(gameObject, 5f);
    }

    public void AttachListeners()
    {
        _healthController.OnDeath += OnDeath;
        _healthController.OnDamaged += OnDamaged;
    }

    public void DetachListeners()
    {
        _healthController.OnDeath -= OnDeath;
        _healthController.OnDamaged -= OnDamaged;

    }
}
