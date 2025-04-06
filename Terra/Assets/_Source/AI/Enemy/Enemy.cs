using _Source.AI.Enemy;
using Terra.StateMachine;
using Platformer;
using Terra.Combat;
using Terra.Player;
using UnityEngine;
using UnityEngine.AI;

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDetector))]
    public class Enemy : Entity, IInitializable, IDamagable
    {
        [SerializeField] NavMeshAgent agent;
        [SerializeField] PlayerDetector playerDetector;
        [SerializeField] Animator animator;
        
        [SerializeField] float wanderRadius = 10f; 
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
            
            attackTimer = new CountdownTimer(timeBetweenAttacks);
            
            stateMachine = new StateMachine();
            
            var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
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

        void Update() {
            stateMachine.Update();
            attackTimer.Tick(Time.deltaTime);
        }
        
        void FixedUpdate() {
            stateMachine.FixedUpdate();
        }
        
        public void Attack() {
            if (attackTimer.IsRunning) return;
            
            attackTimer.Start();
            PlayerManager.Instance.TakeDamage(10f);
        }



        public void TakeDamage(float amount)
        {
            if(!CanBeDamaged) return;
        }


        public void OnDeath()
        {
            //stateMachine.SetState(deathState);
        }
    }
