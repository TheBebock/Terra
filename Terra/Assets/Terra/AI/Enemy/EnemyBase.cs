using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Terra.AI.Data;
using Terra.AI.EnemyStates;
using Terra.Combat;
using Terra.Core.Generics;
using Terra.Core.ModifiableValue;
using Terra.EffectsSystem;
using Terra.Enums;
using Terra.FSM;
using Terra.Interfaces;
using Terra.Managers;
using Terra.Particles;
using Terra.Player;
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
    public abstract class Enemy<TEnemyData> : EnemyBase
        where TEnemyData : EnemyData
    {
        protected abstract TEnemyData Data { get; }

        public sealed override float AttackRange => Data.attackRange;
        
    }

    /// <summary>
    ///     Represents base class for all enemies, handling health, state machine, and animations.
    /// </summary>
    public abstract class EnemyBase : Entity, IDamageable, IAttachListeners
    {
        [SerializeField] private float _deathFadeDuration = 4;
        [SerializeField] private AnimationCurve _deathFadeCurve;
        [FormerlySerializedAs("enemyStats")]
        [Header("Stats")] 
        [SerializeField, Expandable] protected EnemyStatsDefinition _enemyStats;

        [Foldout("References")][SerializeField] protected SpriteRenderer _shadow;
        [FormerlySerializedAs("agent")] [Foldout("References")][SerializeField] protected NavMeshAgent _agent;
        [FormerlySerializedAs("animator")] [Foldout("References")][SerializeField] protected Animator _animator;
        [FormerlySerializedAs("enemyCollider")] [Foldout("References")][SerializeField] protected Collider _enemyCollider;
        [FormerlySerializedAs("enemyModel")] [Foldout("References")][SerializeField] protected SpriteRenderer _enemyModel;
        [Foldout("References")][SerializeField] protected AudioSource _audioSource;
        [Foldout("References")][SerializeField] protected Rigidbody _enemyRigidBody;
        [Foldout("Debug"), ReadOnly] [SerializeField] private HealthController _healthController;
        [Foldout("Debug"), ReadOnly] [SerializeField] private StatusContainer _statusContainer;
        
        [FormerlySerializedAs("hurtSFX")] [Foldout("SFX")] [SerializeField] protected AudioClip _hurtSFX;
        [FormerlySerializedAs("deathSFX")] [Foldout("SFX")] [SerializeField] protected AudioClip _deathSFX;
        
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
        public bool CanBeDamaged => _healthController.CurrentHealth > 0f && !_healthController.IsImmuneAfterHit;
        public abstract float AttackRange { get; }


        protected Vector3 ItemsSpawnPosition => new(
            transform.position.x, 
            transform.position.y - 0.5f,
            transform.position.z - 1f);
        
        /// <summary>
        /// Initializes health, timer, and builds the AI state machine.
        /// </summary>
        protected virtual void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            if (_enemyStats == null)
            {
                Debug.LogError($"[{nameof(EnemyBase)}] Missing EnemyStatsDefinition on {name}. Please assign it in the inspector.");
                return;
            }

            _agent.speed = _enemyStats.baseDexterity;
            _statusContainer = new StatusContainer(this);
            _healthController = new HealthController(new ModifiableValue(_enemyStats.baseMaxHealth), CancellationToken);

            stateMachine = new StateMachine();
            
            enemyDeathState = new EnemyDeathState(this, _agent, _animator);
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

            transform.rotation = Quaternion.identity;
            InternalUpdate();

            StatusContainer.UpdateEffects();
            stateMachine.Update();
            
            UpdateFacingDirection();
        }

        protected virtual void InternalUpdate()
        {
            
        }
        /// <summary>
        /// Updates facing direction based on agent velocity.
        /// </summary>
        private void UpdateFacingDirection()
        {
            if(isDead) return;
            
            float vx = _agent.velocity.x;
            float vxAbs = Mathf.Abs(vx);
            float directionChangeThreshold = 0.02f;
            
            if(vxAbs < directionChangeThreshold) return;
            
            FacingDirection newDirection = vx > 0 ? FacingDirection.Right : FacingDirection.Left;

            
            CurrentDirection = newDirection;
            _animator.SetInteger(AnimationHashes.Direction, (int)CurrentDirection);
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
        

        protected virtual bool CanAttackPlayer() {
            return Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) <= AttackRange;
        }
        
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
            
            AudioManager.Instance.PlaySFXAtSource(_hurtSFX, _audioSource);
            
            // Prevent negative damage values
            if (amount < 0) amount = 0;

            _healthController.TakeDamage(amount, isPercentage);
            PopupDamageManager.Instance.UsePopup(transform, Quaternion.identity, amount);
            
            VFXController.SpawnAndAttachParticleToEntity(this, VFXcontroller.onHitParticle);
            VFXcontroller.BlinkModelsColor(Color.red, 0.15f, 0.1f, 0.15f);
        }

        public void Kill(bool isSilent = true) => _healthController.Kill(isSilent);

        void IDamageable.OnDeath()
        {
            InternalOnDeath();
        }

        /// <summary>
        /// Handles death behavior and schedules cleanup.
        /// </summary>
        private void InternalOnDeath()
        {
            if (isDead) return;
            
            AudioManager.Instance.PlaySFXAtSource(_deathSFX, _audioSource);
            isDead = true;
            _enemyCollider.enabled = false;
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _agent.enabled = false;
            _enemyRigidBody.isKinematic = true;
            
            if(_shadow) _shadow.DOFade(0, _deathFadeDuration);
            VFXcontroller.DoFadeModel(0, _deathFadeDuration, _deathFadeCurve);
            VFXController.SpawnAndAttachParticleToEntity(this, VFXcontroller.onDeathParticle);

            SpawnLootOnDeath();
            OnDeath();

            _ = DestroyObj();
        }

        private async UniTaskVoid DestroyObj()
        {
            await UniTask.WaitForSeconds(_deathFadeDuration + 0.2f, cancellationToken: CancellationToken);
            BeforeDeletion();
            await UniTask.WaitForSeconds(0.1f, cancellationToken: CancellationToken);
            Destroy(gameObject);
        }

        protected virtual void BeforeDeletion()
        {
            
        }
        protected virtual void OnDeath(){}

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

            if (!_enemyRigidBody)
            {
                _enemyRigidBody = GetComponent<Rigidbody>();
                if (_enemyRigidBody == null)
                    Debug.LogError($"[{name}] Missing Rigidbody component.", this);
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

            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
                if (_audioSource == null)
                {
                    Debug.LogError($"[{name}] Missing AudioSource.", this);
                }
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
