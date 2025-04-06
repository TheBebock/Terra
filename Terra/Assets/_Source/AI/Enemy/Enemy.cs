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
    
    [SerializeField] float detectionRadius = 5f;  // Zasięg wykrywania gracza
    [SerializeField] float attackRadius = 1.5f;   // Zasięg, w którym wróg może zaatakować
    [SerializeField] float timeBetweenAttacks = 1f;
    
    [SerializeField] private HealthController _healthController;
    public HealthController HealthController => _healthController;
    
    public bool IsInvincible => _healthController.IsInvincible;
    public bool CanBeDamaged { get; }
    
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
        
        At(wanderState, chaseState, new FuncPredicate(() => {
            Debug.Log("Przejście: Wander → Chase");
            return playerDetector.CanDetectPlayer();
        }));
        At(chaseState, wanderState, new FuncPredicate(() => {
            Debug.Log("Przejście: Chase → Wander");
            return !playerDetector.CanDetectPlayer();
        }));
        At(chaseState, attackState, new FuncPredicate(() => {
            Debug.Log("Przejście: Chase → Attack");
            return playerDetector.CanAttackPlayer();
        }));
        At(attackState, chaseState, new FuncPredicate(() => {
            Debug.Log("Przejście: Attack → Chase");
            return !playerDetector.CanAttackPlayer();
        }));

        stateMachine.SetState(wanderState);
    }
    
    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    void Update() {
        stateMachine.Update();
        attackTimer.Tick(Time.deltaTime);
    }
    
    void FixedUpdate() {
        stateMachine.FixedUpdate();
    }
    
    public void Attack() 
    {
        if (attackTimer.IsRunning) return;

        attackTimer.Start();

        // Upewniamy się, że nie sprawdzamy wysokości
        Vector3 targetPosition = PlayerManager.Instance.transform.position;
        targetPosition.y = transform.position.y; // Ignorujemy różnice w wysokości

        float distanceToPlayer = Vector3.Distance(transform.position, targetPosition);
        Debug.Log($"Odległość do gracza: {distanceToPlayer}, zasięg ataku: {attackRadius}");

        // Jeśli gracz jest w zasięgu ataku
        if (distanceToPlayer <= attackRadius)
        {
            // Wyznaczamy kierunek do gracza
            Vector3 targetDir = (PlayerManager.Instance.transform.position - transform.position).normalized;

            // Sprawdzamy kąt, by upewnić się, że wróg patrzy na gracza
            float angle = Vector3.Angle(transform.forward, targetDir);
            Debug.Log($"Kąt między wrogiem a graczem: {angle}");

            if (angle < 30f) // Możesz zmienić ten kąt
            {
                // Obliczamy pozycję przed przeciwnikiem w kierunku gracza
                Vector3 przedObiektem = transform.position + transform.forward * 1.5f;

                // Szukamy celów w tej pozycji (np. w promieniu 1.5f przed wrogiem)
                var trafieni = ContactProvider.GetTargetsInSphere<IDamagable>(
                    przedObiektem,
                    1.5f,
                    ContactProvider.PlayerTargetsMask
                );

                if (trafieni.Count > 0)
                {
                    CombatManager.Instance.EnemyPerformedAttack(trafieni, 10f);
                    Debug.Log("Wróg trafił przeciwnika!");
                }
                else
                {
                    Debug.Log("Brak celu do ataku");
                }
            }
            else
            {
                Debug.Log("Wróg nie patrzy w stronę gracza, nie wykonuje ataku");
            }
        }
        else
        {
            Debug.Log("Gracz jest poza zasięgiem ataku.");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
    
        // Rysujemy zasięg wykrywania
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Rysujemy zasięg ataku
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void TakeDamage(float amount)
    {
        if (!CanBeDamaged) return;
        _healthController.TakeDamage(amount);
    }

    public void OnDeath()
    {
        //stateMachine.SetState(deathState);
    }
}
