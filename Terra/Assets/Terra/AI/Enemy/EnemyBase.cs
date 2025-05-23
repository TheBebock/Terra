using DG.Tweening;
using NaughtyAttributes;
using Terra.AI.Data.Definitions;
using Terra.AI.EnemyStates;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.Core.ModifiableValue;
using Terra.EffectsSystem;
using Terra.Enums;
using Terra.FSM;
using Terra.Interfaces;
using Terra.StatisticsSystem.Definitions;
using Terra.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.Enemy
{
    /// <summary>
    ///     Represents enemy that has data attached to it
    /// </summary>
    public abstract class Enemy<TEnemyData> : EnemyBase, IWithSetUp
        where TEnemyData : EnemyData
    {
        protected abstract TEnemyData Data { get; }
        public virtual void SetUp()
        {
            playerDetector.Init(Data);
        }
        public virtual void TearDown()
        {
            
        }
    }

    /// <summary>
    ///     Represents base class for all enemies, handling health, state machine, and animations.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent), typeof(PlayerDetector))]
    public abstract class EnemyBase : Entity, IDamageable, IAttachListeners
    {


        [Header("Stats")] 
        [SerializeField, Expandable] protected EnemyStatsDefinition enemyStats;

        [Foldout("References")][SerializeField] protected NavMeshAgent agent;
        [Foldout("References")][SerializeField] protected PlayerDetector playerDetector;
        [Foldout("References")][SerializeField] protected Animator animator;
        [Foldout("References")][SerializeField] protected Collider enemyCollider;
        [Foldout("References")][SerializeField] protected SpriteRenderer enemyModel;

        [Foldout("Debug"), ReadOnly] [SerializeField] private HealthController _healthController;
        [Foldout("Debug"), ReadOnly] [SerializeField] private StatusContainer _statusContainer;
        protected StateMachine stateMachine;
        protected EnemyDeathState enemyDeathState;
        protected CountdownTimer attackTimer;
        private bool _stateMachineLocked;
        protected bool isDead;
        private bool CanUpdateState => !isDead || !_stateMachineLocked;

        public HealthController HealthController => _healthController;
        public FacingDirection CurrentDirection { get; private set; } = FacingDirection.Right;
        public StatusContainer StatusContainer => _statusContainer;
        public bool IsInvincible => _healthController.IsInvincible;
        public bool CanBeDamaged => _healthController.CurrentHealth > 0f;
        public abstract float AttackRange { get; }

        /// <summary>
        /// Initializes health, timer, and builds the AI state machine.
        /// </summary>
        protected virtual void Awake()
        {
            if (enemyStats == null)
            {
                Debug.LogError($"[{nameof(EnemyBase)}] Missing EnemyStatsDefinition on {name}. Please assign it in the inspector.");
                return;
            }

            _statusContainer = new StatusContainer(this);
            _healthController = new HealthController(new ModifiableValue(enemyStats.baseMaxHealth));
            attackTimer = new CountdownTimer(GetAttackCooldown());

            AttachListeners();

            stateMachine = new StateMachine();
            
            enemyDeathState = new EnemyDeathState(this, agent, animator);
            stateMachine.AddAnyTransition(enemyDeathState, new FuncPredicate(() => isDead));
            
            SetupStates();
        }

        /// <summary>
        /// Configures the state machine (to be implemented by subclasses).
        /// </summary>
        protected abstract void SetupStates();

        protected void Update()
        {
            if (!CanUpdateState) return;

            StatusContainer.UpdateEffects();
            stateMachine.Update();
            attackTimer.Tick(Time.deltaTime);

            UpdateFacingDirection(playerDetector.transform);

        }

        /// <summary>
        /// Updates facing direction based on agent velocity.
        /// </summary>
        public void UpdateFacingDirection(Transform player)
        {
            float vx = agent.velocity.x;
            float directionChangeThreshold = 0.05f;

            // Use Mathf.Sign to simplify direction change logic
            FacingDirection newDirection = vx > directionChangeThreshold ? FacingDirection.Right :
                                          vx < -directionChangeThreshold ? FacingDirection.Left : CurrentDirection;

            if (newDirection != CurrentDirection)
            {
                CurrentDirection = newDirection;
                animator.SetInteger(AnimationHashes.Direction, (int)CurrentDirection);
            }
        }

        /// <summary>
        /// Abstract method for performing an attack, to be implemented by subclasses.
        /// </summary>
        public abstract void AttemptAttack();

        /// <summary>
        /// Returns the cooldown duration between attacks, to be implemented by subclasses.
        /// </summary>
        protected abstract float GetAttackCooldown();

        /// <summary>
        /// Applies damage, triggers visual feedback and damage popup.
        /// </summary>
        public void TakeDamage(float amount, bool isPercentage = false)
        {
            if (!CanBeDamaged)
            {
                Debug.Log("Enemy is invincible and cannot take damage.");
                return;
            }

            // Prevent negative damage values
            if (amount < 0f) amount = 0f;

            _healthController.TakeDamage(amount, isPercentage);
            PopupDamageManager.Instance.UsePopup(transform.position, Quaternion.identity, amount);

            // Flash red when damaged for feedback
            enemyModel.material.DOColor(Color.red, 0.25f).SetLoops(2, LoopType.Yoyo);
        }

        public void Kill(bool isSilent = true) => _healthController.Kill(isSilent);

        void IDamageable.OnDeath()
        {
            OnDeath();
        }

        /// <summary>
        /// Handles death behavior and schedules cleanup.
        /// </summary>
        private void OnDeath()
        {
            if (isDead) return;

            isDead = true;
            enemyCollider.enabled = false;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.enabled = false;

            Destroy(gameObject, 5f); 
        }

        public virtual void AttachListeners()
        {
            _healthController.OnDeath += (this as IDamageable).OnDeath;
        }

        public virtual void DetachListeners()
        {
            _healthController.OnDeath -= (this as IDamageable).OnDeath;
        }

        public void SetCanUpdateState(bool value)
        {
            _stateMachineLocked = value;
        }

        /// <summary>
        /// Validates and checks if all required components are assigned in the inspector.
        /// </summary>
        protected virtual void OnValidate()
        {
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
                if (agent == null)
                    Debug.LogError($"[{name}] Missing NavMeshAgent component.", this);
            }

            if (playerDetector == null)
            {
                playerDetector = GetComponent<PlayerDetector>();
                if (playerDetector == null)
                    Debug.LogError($"[{name}] Missing PlayerDetector component.", this);
            }

            if (animator == null)
            {
                animator = GetComponent<Animator>();
                if (animator == null)
                    Debug.LogError($"[{name}] Missing Animator component.", this);
            }

            if (enemyCollider == null)
            {
                enemyCollider = GetComponent<Collider>();
                if (enemyCollider == null)
                    Debug.LogError($"[{name}] Missing Collider component.", this);
            }

            if (enemyModel == null)
            {
                enemyModel = GetComponentInChildren<SpriteRenderer>();
                if (enemyModel == null)
                    Debug.LogError($"[{name}] Missing SpriteRenderer (enemyModel) in children.", this);
            }
        }
        
    }
}
