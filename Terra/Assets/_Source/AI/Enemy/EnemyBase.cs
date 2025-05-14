using System;
using _Source.AI.Data.Definitions;
using _Source.AI.Enemy;
using AI.Data.Definitions;
using Core.ModifiableValue;
using DG.Tweening;
using NaughtyAttributes;
using StatisticsSystem.Definitions;
using Terra.AI.EnemyStates;
using Terra.AI.States.EnemyStates;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.EffectsSystem;
using Terra.EffectsSystem.Abstracts;
using Terra.Enums;
using Terra.FSM;
using Terra.Interfaces;
using Terra.Utils;
using UnityEngine;
using UnityEngine.AI;


namespace Terra.AI.Enemies
{

    /// <summary>
    ///     Represents base class for all 
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent), typeof(PlayerDetector))]
    public abstract class EnemyBase : Entity, IDamageable, IAttachListeners
    {
        protected static readonly int DirectionHash = Animator.StringToHash("Direction");
        
        [Header("AI Data")]
        [SerializeField, Expandable] private EnemyData enemyData;

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
        protected bool stateMachineLocked = false;
        protected bool isDead = false;
        public EnemyData EnemyData => enemyData;



        public HealthController HealthController => _healthController;
        public FacingDirection CurrentDirection { get; set; } = FacingDirection.Right;
        public StatusContainer StatusContainer => _statusContainer;
        public bool IsInvincible => _healthController.IsInvincible;
        public bool CanBeDamaged => _healthController.CurrentHealth > 0f;

        /// <summary>
        /// Initialize health, timer, and build the AI state machine.
        /// </summary>
        private void Awake()
        {
           
            if (enemyStats == null)
            {
                Debug.LogError($"[{nameof(EnemyBase)}] missing EnemyStatsDefinition on {name}");
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
        /// Configures the state machine
        /// </summary>
        protected abstract void SetupStates();

        protected virtual void Update()
        {
            if (isDead || stateMachineLocked) return;
            StatusContainer.UpdateEffects();
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
        private void UpdateFacingDirection()
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
        /// Applies damage, triggers feedback and popup.
        /// </summary>
        public void TakeDamage(float amount, bool isPercentage = false)
        {
            if (!CanBeDamaged) return;
            _healthController.TakeDamage(amount, isPercentage);
            PopupDamageManager.Instance.UsePopup(transform.position, Quaternion.identity, amount);
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
            
            //TODO: Remove this and add normal death animations
            animator.enabled = false;
            enemyModel.material.DOFade(0f, 3.5f);
            stateMachine.SetState(enemyDeathState);
            
            Destroy(gameObject, 5f);
        }

        public void AttachListeners()
        {
            _healthController.OnDeath += (this as IDamageable).OnDeath;
        }

        public void DetachListeners()
        {
            _healthController.OnDeath -= (this as IDamageable).OnDeath;
        }

        public void SetCanUpdateState(bool value)
        {
            stateMachineLocked = value;
        }

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