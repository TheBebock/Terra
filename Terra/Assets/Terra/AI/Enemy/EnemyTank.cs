using NaughtyAttributes;
using Terra.AI.Data;
using Terra.AI.EnemyStates;
using Terra.Enums;
using Terra.Environment;
using Terra.FSM;
using Terra.Player;
using Terra.Utils;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class EnemyTank : Enemy<TankEnemyData>
    {
        [SerializeField] DamageableStatue _tankStatue;
        [SerializeField] private Transform _statueSpawnPointLeft;
        [SerializeField] private Transform _statueSpawnPointRight;
        [SerializeField] private EnemyColliderComponent _attackCollider;
        [SerializeField, Expandable] private TankEnemyData _data;
        [SerializeField] LayerMask _groundObjectLayerMask;
        private int _targetLayer;

        private CountdownTimer _attackCooldownTimer;
        private CountdownTimer _attackDurationTimer;
        protected override TankEnemyData Data => _data;

        public LayerMask GroundObjectLayerMask => _groundObjectLayerMask;
        public int TargetLayer => _targetLayer;
        public EnemyColliderComponent AttackCollider => _attackCollider;

        protected override void Awake()
        {
            base.Awake();
            _targetLayer = LayerMask.NameToLayer("Ground");
            _attackCollider.Init(this, _enemyStats.baseStrength, _deafultAttackSFX, _audioSource);
            _attackCollider.DisableCollider();
        }

        protected override void SetupStates()
        {
            var walkAndAttack = new EnemyWalkAndAttackState(this, _agent, _animator, 
                PlayerManager.Instance.PlayerEntity, _data.normalAttackRange);            
            var tired = new EnemyTankTiredState(this, _agent, _animator);

            _attackCooldownTimer = new CountdownTimer(Data.attackCooldown);
            _attackDurationTimer = new CountdownTimer(Data.attackDuration);

            _attackDurationTimer.OnTimerStop += OnAttackDurationTimerFinished;
            _attackCooldownTimer.OnTimerStop += OnAttackCooldownTimerFinished;
            
            _attackCooldownTimer.Start();
            
            stateMachine.AddTransition(walkAndAttack, tired, new FuncPredicate(CanEnterTiredState));
            stateMachine.AddTransition(tired, walkAndAttack, new FuncPredicate(CanAttackPlayer));

            stateMachine.SetState(tired);
        }
        
        /// <summary>
        ///     When the duration of attack has ended, reset time of attackDuration without turning it on
        ///     and restart cooldown timer.
        /// </summary>
        private void OnAttackDurationTimerFinished()
        {
            _attackDurationTimer?.ResetTime();
            _attackCooldownTimer.Restart();
        }

        /// <summary>
        ///     When the attack cooldown has finished, start the attackDuration timer
        /// </summary>
        private void OnAttackCooldownTimerFinished()
        {
            _attackDurationTimer?.Start();
        }
        
        protected override void InternalUpdate()
        {
            base.InternalUpdate();
            _attackCooldownTimer?.Tick(Time.deltaTime);
            _attackDurationTimer?.Tick(Time.deltaTime);
        }
        
        protected override bool CanAttackPlayer()
        {
            return _attackCooldownTimer.IsFinished && _attackDurationTimer.IsRunning;
        }

        private bool CanEnterTiredState()
        {
            return !_attackCooldownTimer.IsFinished;
        }

        protected override void SpawnLootOnDeath()
        {
            //Nothing
        }
        
        protected override void BeforeDeletion()
        {
            base.BeforeDeletion();
            SpawnStatue();
        }

        public override void AttemptAttack()
        {
            //Noop
        }

        private void SpawnStatue()
        {
            switch (CurrentDirection)
            {
                case FacingDirection.Left:
                    Instantiate(_tankStatue, _statueSpawnPointLeft.position, Quaternion.identity).Init(CurrentDirection);
                    break;
                case FacingDirection.Right:
                    Instantiate(_tankStatue, _statueSpawnPointRight.position, Quaternion.identity).Init(CurrentDirection);
                    break;
            }
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            _attackDurationTimer.OnTimerStop -= OnAttackDurationTimerFinished;
            _attackCooldownTimer.OnTimerStop -= OnAttackCooldownTimerFinished;
        }
    }
}
