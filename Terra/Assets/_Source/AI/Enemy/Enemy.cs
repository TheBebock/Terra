using System.Collections.Generic;
using _Source.AI.Enemy;
using Terra.StateMachine;
using Terra.AI.States.EnemyStates;
using Terra.Combat;
using Terra.Player;
using Terra.Utils;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PlayerDetector))]
public class Enemy : Entity, IInitializable, IDamagable
{
    private static readonly int Direction = Animator.StringToHash("Direction");
    [SerializeField] NavMeshAgent agent;
    [SerializeField] PlayerDetector playerDetector;
    [SerializeField] Animator animator;
    
    [SerializeField] float detectionRadius = 5f;
    [SerializeField] float attackRadius = 1.5f;
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] private float attackDamage = 1;
    [SerializeField] private HealthController _healthController;
    public HealthController HealthController => _healthController;
    public enum FacingDirection { Left, Right }
    public FacingDirection CurrentDirection { get; private set; } = FacingDirection.Right;
    public bool IsInvincible => _healthController.IsInvincible;
    public bool CanBeDamaged => !_healthController.IsInvincible && _healthController.CurrentHealth > 0f;

    private bool isDead = false;
    
    StateMachine stateMachine;
    CountdownTimer attackTimer;

    public bool IsInitialized { get; set; }

    public void Initialize()
    {
        if (agent == null || playerDetector == null || animator == null)
        {
            Debug.LogError("Brak wymaganych komponentów!");
            return;
        }

        attackTimer = new CountdownTimer(timeBetweenAttacks);

        stateMachine = new StateMachine();

        var wanderState = new EnemyWanderState(this, animator, agent, detectionRadius);
        var chaseState = new EnemyChaseState(this, animator, agent, PlayerManager.Instance.transform);
        var attackState = new EnemyAttackState(this, animator, agent, PlayerManager.Instance.transform);
        var deathState = new EnemyDeathState(this, animator); 
        
        Any(deathState, new FuncPredicate(() => _healthController.IsDead));
        
        At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
        At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
        At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
        At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));

        stateMachine.SetState(wanderState);
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    void Update()
    {
        if(isDead) return;
        
        stateMachine.Update();
        attackTimer.Tick(Time.deltaTime);
        UpdateFacingDirection();
    }
    
    public void UpdateFacingDirection()
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
    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public void Attack()
    {
        Vector3 attackOrigin = transform.position + Vector3.up; 

        var targets = ContactProvider.GetTargetsInSphere<IDamagable>(transform.position, attackRadius, ContactProvider.EnemyTargetsMask);

        foreach (var target in targets)
        {
            target.TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void TakeDamage(float amount)
    {
        if (!CanBeDamaged) return;

        _healthController.TakeDamage(amount);
        PopupDamageManager.Instance.CreatePopup(transform.position, Quaternion.identity, amount);
    }

    public void OnDeath()
    {
        Debug.Log("Enemy died.");        
        animator.enabled = false;
        agent.isStopped = true;
        isDead = true;
    }
}
