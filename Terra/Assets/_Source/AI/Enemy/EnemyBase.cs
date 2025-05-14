using _Source.AI.Data.Definitions;
using _Source.AI.EnemyStates;
using Core.ModifiableValue;
using DG.Tweening;
using NaughtyAttributes;
using StatisticsSystem.Definitions;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.EffectsSystem;
using Terra.Enums;
using Terra.FSM;
using Terra.Interfaces;
using Terra.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace _Source.AI.Enemy
{
    /// <summary>
    ///     Represents enemy that has data attached to it
    /// </summary>
    public abstract class Enemy<TEnemyData> : EnemyBase
        where TEnemyData : EnemyData
    {
        protected abstract TEnemyData Data { get; }
    }

    /// <summary>
    ///     Represents base class for all enemies, handling health, state machine, and animations.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent), typeof(PlayerDetector))]
    public abstract class EnemyBase :  Entity, IDamageable, IAttachListeners
    {
        public TeamType GetTeam() => TeamType.Enemy;
        private static readonly int DirectionHash = Animator.StringToHash("Direction");
        private static readonly int Death = Animator.StringToHash("Death");

        [Header("Stats")] 
        [SerializeField, Expandable] protected EnemyStatsDefinition enemyStats;

        [Foldout("References")][SerializeField] protected NavMeshAgent agent;
        [Foldout("References")][SerializeField] protected PlayerDetector playerDetector;
        [Foldout("References")][SerializeField] protected Animator animator;
        [Foldout("References")][SerializeField] protected Collider enemyCollider;
        [Foldout("References")][SerializeField] protected SpriteRenderer enemyModel;

        [Foldout("Debug"), ReadOnly] [SerializeField] private HealthController healthController;
        [Foldout("Debug"), ReadOnly] [SerializeField] private StatusContainer statusContainer;
        protected StateMachine StateMachine;
        protected EnemyDeathState EnemyDeathState;
        protected CountdownTimer AttackTimer;
        private bool _stateMachineLocked;
        protected bool IsDead;
        private bool CanUpdateState => !IsDead && !_stateMachineLocked;

        public HealthController HealthController => healthController;
        public FacingDirection CurrentDirection { get; private set; } = FacingDirection.Right;
        public StatusContainer StatusContainer => statusContainer;
        public bool IsInvincible => healthController.IsInvincible;
        public bool CanBeDamaged => healthController.CurrentHealth > 0f;
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

            statusContainer = new StatusContainer(this);
            healthController = new HealthController(new ModifiableValue(enemyStats.baseMaxHealth));
            AttackTimer = new CountdownTimer(GetAttackCooldown());

            AttachListeners();

            StateMachine = new StateMachine();
            
            EnemyDeathState = new EnemyDeathState(this, agent, animator);
            StateMachine.AddAnyTransition(EnemyDeathState, new FuncPredicate(() => IsDead));
            
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
            StateMachine.Update();
            AttackTimer.Tick(Time.deltaTime);

            // Player detection and attack logic
            if (playerDetector != null && playerDetector.CanDetectPlayer())
            {
                if (playerDetector.CanAttackPlayer())
                {
                    AttemptAttack();
                }

                UpdateFacingDirection(playerDetector.transform);
            }
        }

        protected virtual void FixedUpdate()
        {
            if (IsDead || _stateMachineLocked) return;
            StateMachine.FixedUpdate();
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
                animator.SetInteger(DirectionHash, (int)CurrentDirection);
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

            healthController.TakeDamage(amount, isPercentage);
            PopupDamageManager.Instance.UsePopup(transform.position, Quaternion.identity, amount);

            // Flash red when damaged for feedback
            enemyModel.material.DOColor(Color.red, 0.25f).SetLoops(2, LoopType.Yoyo);
        }

        public void Kill(bool isSilent = true) => healthController.Kill(isSilent);

        void IDamageable.OnDeath()
        {
            OnDeath();
        }

        /// <summary>
        /// Handles death behavior and schedules cleanup.
        /// </summary>
        private void OnDeath()
        {
            if (IsDead) return;

            IsDead = true;
            enemyCollider.enabled = false;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.enabled = false;
            
            animator.SetTrigger(Death);

            // Optionally, you could replace this with object pooling if you have many enemies being destroyed.
            Destroy(gameObject, 5f); 
        }

        public virtual void AttachListeners()
        {
            healthController.OnDeath += (this as IDamageable).OnDeath;
        }

        public virtual void DetachListeners()
        {
            healthController.OnDeath -= (this as IDamageable).OnDeath;
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
