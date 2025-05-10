using _Source.AI.Enemy;
using Core.ModifiableValue;
using DG.Tweening;
using StatisticsSystem.Definitions;
using Terra.AI.States.EnemyStates;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.StateMachine;
using Terra.Interfaces;
using Terra.Utils;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Abstract base class for all enemies: handles health, state machine, facing, damage and death.
/// </summary>
[RequireComponent(typeof(NavMeshAgent), typeof(PlayerDetector))]
public abstract class EnemyBase : Entity, IDamagable, IAttachListeners
{
    protected static readonly int DirectionHash = Animator.StringToHash("Direction");

    [Header("Components")] 
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected PlayerDetector playerDetector;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Collider enemyCollider;
    [SerializeField] protected SpriteRenderer enemyModel;

    [Header("Stats")]
    [SerializeField] protected EnemyStatsDefinition enemyStats;

    [Header("Behavior Settings")]
    [Tooltip("Distance at which the enemy detects the player and transitions states.")]
    [SerializeField] protected float detectionRadius = 5f;

    protected HealthController _healthController;
    protected StateMachine stateMachine;
    protected EnemyDeathState enemyDeathState;
    protected CountdownTimer attackTimer;
    protected bool stateMachineLocked = false;
    protected bool isDead = false;

    public enum FacingDirection { Left, Right }
    public FacingDirection CurrentDirection { get;  set; } = FacingDirection.Right;

    public bool IsInvincible => _healthController.IsInvincible;
    public bool CanBeDamaged => _healthController.CurrentHealth > 0f;

    /// <summary>
    /// Initialize health, timer, and build the AI state machine.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        if (enemyStats == null)
        {
            Debug.LogError($"[{nameof(EnemyBase)}] missing EnemyStatsDefinition on {name}");
        }

        _healthController = new HealthController(new ModifiableValue(enemyStats.baseMaxHealth));
        attackTimer = new CountdownTimer(GetAttackCooldown());

        AttachListeners();

        stateMachine = new StateMachine();
        SetupStates();
    }
    

    protected virtual void Update()
    {
        if (isDead || stateMachineLocked) return;
        stateMachine.Update();
        attackTimer.Tick(Time.deltaTime);
        UpdateFacingDirection();
    }

    protected virtual void FixedUpdate()
    {
        if (isDead || stateMachineLocked) return;
        stateMachine.FixedUpdate();
    }

    /// <summary>
    /// Updates facing based on agent velocity.
    /// </summary>
    protected void UpdateFacingDirection()
    {
        float vx = agent.velocity.x;
        if (vx > 0.05f)
        {
            CurrentDirection = FacingDirection.Right;
            animator.SetInteger(DirectionHash, 1);
        }
        else if (vx < -0.05f)
        {
            CurrentDirection = FacingDirection.Left;
            animator.SetInteger(DirectionHash, 0);
        }
    }

    /// <summary>
    /// Called to perform an attack; implemented by subclasses.
    /// </summary>
    public abstract void AttemptAttack();

    /// <summary>
    /// Returns cooldown in seconds between attacks; implemented by subclasses.
    /// </summary>
    protected abstract float GetAttackCooldown();

    /// <summary>
    /// Configures the state machine (wander, chase, attack, death); implemented by subclasses.
    /// </summary>
    protected abstract void SetupStates();

    /// <summary>
    /// Applies damage, triggers feedback and popup.
    /// </summary>
    public void TakeDamage(float amount, bool isPercentage = false)
    {
        if (!CanBeDamaged) return;
        _healthController.TakeDamage(amount, isPercentage);
        PopupDamageManager.Instance.UsePopup(transform.position, Quaternion.identity, amount);
        enemyModel.material.DOColor(Color.red, 0.25f).SetLoops(2, LoopType.Yoyo);
    }

    public HealthController HealthController { get; }

    /// <summary>
    /// Handles death behavior and schedules cleanup.
    /// </summary>
    public virtual void OnDeath()
    {
        if (isDead) return;
        isDead = true;
        enemyCollider.enabled = false;
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.updateRotation = false;
        animator.enabled = false;
        enemyModel.material.DOFade(0f, 3.5f);
        stateMachine.SetState(enemyDeathState);
        Destroy(gameObject, 5f);
    }

    public void AttachListeners()
    {
        _healthController.OnDeath += OnDeath;
        _healthController.OnDamaged += _ => enemyModel.material.DOColor(Color.red, 0.25f).SetLoops(2, LoopType.Yoyo);
    }

    public void DetachListeners()
    {
        _healthController.OnDeath -= OnDeath;
        _healthController.OnDamaged -= _ => { };
    }

    public void CanUpdateState(bool can)
    {
        stateMachineLocked = !can;
    }
}
