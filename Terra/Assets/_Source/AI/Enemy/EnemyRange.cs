using Core.ModifiableValue;
using DG.Tweening;
using StatisticsSystem.Definitions;
using Terra.AI.States.EnemyStates;
using Terra.Combat;
using Terra.Player;
using Terra.StateMachine;
using Terra.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace _Source.AI.Enemy // ← zmień na właściwą przestrzeń nazw
{
    /// <summary>
    /// Ranged enemy that uses a state machine to wander, chase,
    /// attack from range via BulletFactory, and die.
    /// </summary>
    public class EnemyRange : global::Enemy
    {
        /// <summary>Hash for the Animator "Direction" parameter.</summary>
        private static readonly int DirectionHash = Animator.StringToHash("Direction");

        [Header("Components")]
        [Tooltip("NavMeshAgent used for pathfinding.")]
        [SerializeField] private NavMeshAgent agent;
        [Tooltip("Component that detects the player within given radii.")]
        [SerializeField] private PlayerDetector playerDetector;
        [Tooltip("Animator controlling enemy animations.")]
        [SerializeField] private Animator animator;
        [Tooltip("Collider used to disable on death.")]
        [SerializeField] private Collider enemyCollider;
        [Tooltip("SpriteRenderer for damage feedback.")]
        [SerializeField] private SpriteRenderer enemyModel;
        [Tooltip("Factory that creates and initializes bullets.")]
        [SerializeField] private BulletFactory bulletFactory;
        [Tooltip("Point from which bullets are fired.")]
        [SerializeField] private Transform firePoint;

        [Header("Stats")]
        [Tooltip("Base stats definition (health, strength, etc.).")]
        [SerializeField] private EnemyStatsDefinition enemyStats;

        [Header("Combat Settings")]
        [Tooltip("Configuration for bullet speed, damage, and lifetime.")]
        [SerializeField] private BulletData bulletData;
        [Tooltip("Distance within which the enemy will detect the player and chase.")]
        [SerializeField] private float detectionRadius = 5f;
        [Tooltip("Cooldown between consecutive ranged attacks in seconds.")]
        [SerializeField] private float attackCooldown = 1.5f;

        private HealthController _healthController;
        private CountdownTimer attackTimer;
        private StateMachine stateMachine;
        private EnemyDeathState enemyDeathState;
        private bool stateMachineLocked = false;
        private bool isDead = false;

        /// <summary>Current facing direction of the enemy, used for animation.</summary>
        public enum FacingDirection { Left, Right }
        /// <summary>Gets or sets the current facing direction.</summary>
        public FacingDirection CurrentDirection { get; set; } = FacingDirection.Right;

        /// <summary>Returns true if the enemy is currently invincible.</summary>
        public bool IsInvincible => _healthController.IsInvincible;
        /// <summary>Returns true if the enemy can receive damage.</summary>
        public bool CanBeDamaged => _healthController.CurrentHealth > 0f;

        /// <summary>
        /// Initialize components, health, timers and build the AI state machine.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            if (agent == null || playerDetector == null || animator == null ||
                enemyCollider == null || enemyModel == null || enemyStats == null)
            {
                Debug.LogError("EnemyRange initialization failed: missing component or stats references.");
                return;
            }

            if (bulletFactory == null)
                Debug.LogError("EnemyRange: BulletFactory is not assigned.");
            if (firePoint == null)
                Debug.LogError("EnemyRange: FirePoint transform is not assigned.");

            _healthController = new HealthController(new ModifiableValue(enemyStats.baseMaxHealth));
            attackTimer = new CountdownTimer(attackCooldown);

            // Set up state machine
            stateMachine = new StateMachine();
            var wander = new EnemyWanderState(this, animator, agent, detectionRadius);
            var chase = new EnemyChaseState(this, animator, agent, PlayerManager.Instance.transform);
            var attack = new EnemyRangeAttackState(this, animator, agent, PlayerManager.Instance.transform);
            enemyDeathState = new EnemyDeathState(this, animator);

            stateMachine.AddTransition(wander, chase, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            stateMachine.AddTransition(chase, wander, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            stateMachine.AddTransition(chase, attack, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
            stateMachine.AddTransition(attack, chase, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
            stateMachine.AddAnyTransition(enemyDeathState, new FuncPredicate(() => isDead));
            stateMachine.SetState(wander);
        }

        private void Update()
        {
            if (isDead || stateMachineLocked) return;

            stateMachine.Update();
            attackTimer.Tick(Time.deltaTime);
            UpdateFacing();
        }

        private void FixedUpdate()
        {
            if (isDead || stateMachineLocked) return;
            stateMachine.FixedUpdate();
        }

        /// <summary>
        /// Attempts a ranged attack: if cooldown has elapsed, uses the BulletFactory to spawn a bullet.
        /// </summary>
        public void AttemptAttack()
        {
            if (!attackTimer.IsFinished) return;

            if (bulletFactory == null || firePoint == null)
            {
                Debug.LogError("EnemyRange.AttemptAttack failed: factory or firePoint missing.");
                return;
            }

            var dir = (PlayerManager.Instance.transform.position - firePoint.position).normalized;
            bulletFactory.CreateBullet(bulletData, firePoint.position, dir);
            attackTimer.Reset();
        }

        /// <summary>
        /// Updates the facing direction based on the NavMeshAgent's horizontal velocity.
        /// </summary>
        private void UpdateFacing()
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
        /// Visual feedback callback invoked when the enemy takes damage.
        /// Flashes the sprite red twice.
        /// </summary>
        /// <param name="amount">Amount of damage received (unused in effect logic).</param>
        private void OnDamaged(float amount)
        {
            enemyModel.material.DOColor(Color.red, 0.25f)
                .SetLoops(2, LoopType.Yoyo);
        }

        /// <summary>
        /// Applies damage via the HealthController and spawns a damage popup.
        /// Hides base method since Enemy.TakeDamage is non-virtual.
        /// </summary>
        /// <param name="amount">Amount of damage to apply.</param>
        public new void TakeDamage(float amount)
        {
            if (!CanBeDamaged) return;
            _healthController.TakeDamage(amount);
            PopupDamageManager.Instance.UsePopup(transform.position, Quaternion.identity, amount);
        }

        /// <summary>
        /// Handles death: disables collider & navigation, plays fade-out,
        /// transitions state, and schedules destruction.
        /// Hides base method since Enemy.OnDeath is non-virtual.
        /// </summary>
        public new void OnDeath()
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

        /// <summary>
        /// Subscribes to health events (OnDeath, OnDamaged) on the HealthController.
        /// Hides base method since Enemy.AttachListeners is non-virtual.
        /// </summary>
        public new void AttachListeners()
        {
            _healthController.OnDeath += OnDeath;
            _healthController.OnDamaged += OnDamaged;
        }

        /// <summary>
        /// Unsubscribes from health events on the HealthController.
        /// </summary>
        public new void DetachListeners()
        {
            _healthController.OnDeath -= OnDeath;
            _healthController.OnDamaged -= OnDamaged;
        }
    }
}
