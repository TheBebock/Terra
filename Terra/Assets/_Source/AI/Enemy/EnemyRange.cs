using _Source.AI.Enemy;
using Core.ModifiableValue;
using DG.Tweening;
using StatisticsSystem.Definitions;
using Terra.AI.States.EnemyStates;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.Interfaces;
using Terra.Player;
using Terra.StateMachine;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Ranged enemy behavior class, including state machine handling, attack logic, 
/// and health management.
/// </summary>
public class EnemyRange : Enemy
{
    private static readonly int Direction = Animator.StringToHash("Direction");

    [Header("Components")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayerDetector playerDetector;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider enemyCollider;
    [SerializeField] private SpriteRenderer enemyModel;

    [Header("Stats")]
    [SerializeField] private EnemyStatsDefinition enemyStats;
    private HealthController _healthController;

    [Header("Combat")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float attackRadius = 7f; // Longer attack range for ranged units
    [SerializeField] private float timeBetweenAttacks = 1.5f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    public enum FacingDirection { Left, Right }
    public FacingDirection CurrentDirection { get; set; } = FacingDirection.Right;

    public bool IsInvincible => _healthController.IsInvincible;
    public bool CanBeDamaged => _healthController.CurrentHealth > 0f;
    public HealthController HealthController => _healthController;

    private bool stateMachineUpdateLock = false;
    private bool isDead = false;
    private StateMachine stateMachine;
    private EnemyDeathState enemyDeathState;
    private CountdownTimer attackTimer;

    /// <summary>
    /// Initialization of health, states, and components.
    /// </summary>
    protected virtual void Awake()
    {
        if (agent == null || playerDetector == null || animator == null || enemyStats == null)
        {
            Debug.LogError("EnemyRange initialization failed: missing components or stats.");
            return;
        }

        _healthController = new HealthController(new ModifiableValue(enemyStats.baseMaxHealth));
        attackTimer = new CountdownTimer(timeBetweenAttacks);

        // Set up state machine with transitions
        stateMachine = new StateMachine();
        var wanderState = new EnemyWanderState(this, animator, agent, detectionRadius);
        var chaseState = new EnemyChaseState(this, animator, agent, PlayerManager.Instance.transform);
        var attackState = new EnemyRangeAttackState(this, animator, agent, PlayerManager.Instance.transform);
        enemyDeathState = new EnemyDeathState(this, animator);

        At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
        At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
        At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
        At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));

        Any(enemyDeathState, new FuncPredicate(() => isDead));

        stateMachine.SetState(wanderState);
    }

    private void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    private void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    /// <summary>
    /// Handles regular updates including state logic and attack cooldown.
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
    /// Handles physics-based updates for movement and pathfinding.
    /// </summary>
    private void FixedUpdate()
    {
        if (isDead || stateMachineUpdateLock)
            return;

        stateMachine.FixedUpdate();
    }

    /// <summary>
    /// Performs a ranged attack by instantiating a projectile.
    /// </summary>
    public void AttemptAttack()
    {
        if (!attackTimer.IsFinished || projectilePrefab == null || firePoint == null)
            return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (PlayerManager.Instance.transform.position - firePoint.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 10f; // Customize projectile speed here

        attackTimer.Reset();
    }

    /// <summary>
    /// Visual feedback when damaged (e.g., flashing red).
    /// </summary>
    private void OnDamaged(float value)
    {
        enemyModel.material.DOColor(Color.red, 0.25f)
            .SetLoops(2, LoopType.Yoyo);
    }

    /// <summary>
    /// Applies damage to the enemy and shows damage popup.
    /// </summary>
    public void TakeDamage(float amount)
    {
        if (!CanBeDamaged)
            return;

        Debug.Log($"{gameObject.name} took {amount} damage");
        _healthController.TakeDamage(amount);
        PopupDamageManager.Instance.UsePopup(transform.position, Quaternion.identity, amount);
    }

    /// <summary>
    /// Updates facing direction based on current velocity for animation and logic.
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
    /// Locks or unlocks state machine updates (useful during cutscenes or stuns).
    /// </summary>
    public void CanUpdateState(bool value) => stateMachineUpdateLock = value;

    /// <summary>
    /// Triggers death behavior and starts fade-out/cleanup.
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

        animator.enabled = false;
        enemyModel.material.DOFade(0f, 3.5f);

        stateMachine.SetState(enemyDeathState);
        Destroy(gameObject, 5f);
    }

    /// <summary>
    /// Subscribes to health events.
    /// </summary>
    public void AttachListeners()
    {
        _healthController.OnDeath += OnDeath;
        _healthController.OnDamaged += OnDamaged;
    }

    /// <summary>
    /// Unsubscribes from health events.
    /// </summary>
    public void DetachListeners()
    {
        _healthController.OnDeath -= OnDeath;
        _healthController.OnDamaged -= OnDamaged;
    }
}
