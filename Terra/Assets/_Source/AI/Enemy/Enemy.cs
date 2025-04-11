using System.Collections.Generic;
using _Source.AI.Enemy;
using Terra.StateMachine;
using Platformer;
using Terra.Combat;
using Terra.Player;
using Terra.Utils;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PlayerDetector))]
public class Enemy : Entity, IInitializable, IDamagable
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] PlayerDetector playerDetector;
    [SerializeField] Animator animator;
    
    [SerializeField] float detectionRadius = 5f;
    [SerializeField] float attackRadius = 1.5f;
    [SerializeField] float timeBetweenAttacks = 1f;

    [SerializeField] private HealthController _healthController;
    public HealthController HealthController => _healthController;

    public bool IsInvincible => _healthController.IsInvincible;
    public bool CanBeDamaged => !_healthController.IsInvincible && _healthController.CurrentHealth > 0f;

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
        stateMachine.Update();
        attackTimer.Tick(Time.deltaTime);
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public void Attack()
    {
        if (!attackTimer.IsRunning) // timer gotowy = można bić
        {
            Debug.Log("Enemy próbuje zaatakować!");
            attackTimer.Start();

            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius);
            List<IDamagable> targets = new List<IDamagable>();

            foreach (var col in colliders)
            {
                if (col.TryGetComponent<IDamagable>(out var damagable))
                {
                    if (!(damagable is Enemy)) // nie bijemy innych wrogów
                        targets.Add(damagable);
                }
            }

            if (targets.Count > 0)
            {
                foreach (var target in targets)
                {
                    target.TakeDamage(5f); // bezpośrednio zadawaj obrażenia
                }

                animator.SetTrigger("Attack"); // trigger animacji ataku
            }
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

        if (_healthController.CurrentHealth <= 0f)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        Debug.Log("Enemy died.");
        animator.SetTrigger("Death");
        agent.isStopped = true;
        this.enabled = false;
    }
}
