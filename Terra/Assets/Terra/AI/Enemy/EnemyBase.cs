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
using Terra.Managers;
using Terra.StatisticsSystem.Definitions;
using Terra.Utils;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Terra.AI.Enemy
{
    /// <summary>
    ///     Represents enemy that has data attached to it
    /// </summary>
    public abstract class Enemy<TEnemyData> : EnemyBase, IWithSetUp
        where TEnemyData : EnemyData
    {
        protected abstract TEnemyData Data { get; }

        public sealed override float AttackRange => Data.attackRange;

        public virtual void SetUp()
        {
            _playerDetector.Init(Data);
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
        [FormerlySerializedAs("enemyStats")]
        [Header("Stats")] 
        [SerializeField, Expandable] protected EnemyStatsDefinition _enemyStats;

        [FormerlySerializedAs("agent")] [Foldout("References")][SerializeField] protected NavMeshAgent _agent;
        [FormerlySerializedAs("playerDetector")] [Foldout("References")][SerializeField] protected PlayerDetector _playerDetector;
        [FormerlySerializedAs("animator")] [Foldout("References")][SerializeField] protected Animator _animator;
        [FormerlySerializedAs("enemyCollider")] [Foldout("References")][SerializeField] protected Collider _enemyCollider;
        [FormerlySerializedAs("enemyModel")] [Foldout("References")][SerializeField] protected SpriteRenderer _enemyModel;

        [Foldout("Debug"), ReadOnly] [SerializeField] private HealthController _healthController;
        [Foldout("Debug"), ReadOnly] [SerializeField] private StatusContainer _statusContainer;
        
        [FormerlySerializedAs("hurtSFX")] [Foldout("SFX")] [SerializeField] protected AudioClip _hurtSFX;
        [FormerlySerializedAs("deathSFX")] [Foldout("SFX")] [SerializeField] protected AudioClip _deathSFX;
        
        protected StateMachine StateMachine;
        protected EnemyDeathState EnemyDeathState;
        protected CountdownTimer AttackTimer;
        private bool _stateMachineLocked;
        protected bool IsDead;
        private bool CanUpdateState => !IsDead || !_stateMachineLocked;

        public HealthController HealthController => _healthController;
        public FacingDirection CurrentDirection { get; private set; } = FacingDirection.Right;
        public StatusContainer StatusContainer => _statusContainer;
        public bool IsInvincible => _healthController.IsInvincible;
        public bool CanBeDamaged => _healthController.CurrentHealth > 0f && !_healthController.IsImmuneAfterHit;
        public abstract float AttackRange { get; }
        protected AudioSource AudioSource;

        protected Vector3 ItemsSpawnPosition => new(
            transform.position.x, 
            transform.position.y,
            transform.position.z - 1f);
        
        /// <summary>
        /// Initializes health, timer, and builds the AI state machine.
        /// </summary>
        protected virtual void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            if (_enemyStats == null)
            {
                Debug.LogError($"[{nameof(EnemyBase)}] Missing EnemyStatsDefinition on {name}. Please assign it in the inspector.");
                return;
            }

            _statusContainer = new StatusContainer(this);
            _healthController = new HealthController(new ModifiableValue(_enemyStats.baseMaxHealth), CancellationToken);

            AttackTimer = new CountdownTimer(GetAttackCooldown());

            AttachListeners();

            StateMachine = new StateMachine();
            
            EnemyDeathState = new EnemyDeathState(this, _agent, _animator);
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

            UpdateFacingDirection();

        }

        /// <summary>
        /// Updates facing direction based on agent velocity.
        /// </summary>
        private void UpdateFacingDirection()
        {
            float vx = _agent.velocity.x;
            float directionChangeThreshold = 0.02f;
            
            FacingDirection newDirection = vx > directionChangeThreshold ? FacingDirection.Right : FacingDirection.Left;

            if (newDirection != CurrentDirection)
            {
                CurrentDirection = newDirection;
                _animator.SetInteger(AnimationHashes.Direction, (int)CurrentDirection);
            }
        }
        
        /// <summary>
        /// Updates facing direction based on looking direction.
        /// </summary>
        public void UpdateFacingDirection(Vector3 direction)
        {
            FacingDirection newDirection = direction.x > 0? FacingDirection.Right : FacingDirection.Left;

            if (newDirection != CurrentDirection)
            {
                CurrentDirection = newDirection;
                _animator.SetInteger(AnimationHashes.Direction, (int)CurrentDirection);
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
        public void TakeDamage(int amount, bool isPercentage = false)
        {
            if (!CanBeDamaged)
            {
                Debug.Log("Enemy is invincible and cannot take damage.");
                return;
            }
            
            AudioManager.Instance.PlaySFXAtSource(_hurtSFX, AudioSource);
            
            // Prevent negative damage values
            if (amount < 0) amount = 0;

            _healthController.TakeDamage(amount, isPercentage);
            PopupDamageManager.Instance.UsePopup(transform, Quaternion.identity, amount);
            
            VFXController.PlayParticleOnEntity(VFXController.onHitParticle);
            VFXController.BlinkModelsColor(Color.red, 0.15f, 0.1f, 0.15f);
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
            if (IsDead) return;
            
            AudioManager.Instance.PlaySFXAtSource(_deathSFX, AudioSource);
            IsDead = true;
            _enemyCollider.enabled = false;
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _agent.enabled = false;
            VFXController?.DoFadeModel(0, 4f);
            VFXController?.PlayParticleOnEntity(VFXController.onDeathParticle);

            SpawnLootOnDeath();
            Destroy(gameObject, 5f); 
        }

        /// <summary>
        ///     Spawn crystal pickup on death, override to perform other actions
        /// </summary>
        protected virtual void SpawnLootOnDeath()
        {
            LootManager.Instance.SpawnCrystalPickup(ItemsSpawnPosition);
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

        protected override void OnValidate()
        {
            base.OnValidate();
            if (_agent == null)
            {
                _agent = GetComponent<NavMeshAgent>();
                if (_agent == null)
                    Debug.LogError($"[{name}] Missing NavMeshAgent component.", this);
            }

            if (_playerDetector == null)
            {
                _playerDetector = GetComponent<PlayerDetector>();
                if (_playerDetector == null)
                    Debug.LogError($"[{name}] Missing PlayerDetector component.", this);
            }

            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
                if (_animator == null)
                    Debug.LogError($"[{name}] Missing Animator component.", this);
            }

            if (_enemyCollider == null)
            {
                _enemyCollider = GetComponentInChildren<Collider>();
                if (_enemyCollider == null)
                    Debug.LogError($"[{name}] Missing Collider component.", this);
            }

            if (_enemyModel == null)
            {
                _enemyModel = GetComponentInChildren<SpriteRenderer>();
                if (_enemyModel == null)
                    Debug.LogError($"[{name}] Missing SpriteRenderer (enemyModel) in children.", this);
            }
        }
    }
}
