using JetBrains.Annotations;
using NaughtyAttributes;
using Terra.AI.Data;
using Terra.AI.EnemyStates;
using Terra.AI.EnemyStates.BossStates;
using Terra.Combat;
using Terra.Combat.Projectiles;
using Terra.Enums;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.FSM;
using Terra.Interactions;
using Terra.Managers;
using Terra.Particles;
using Terra.Player;
using Terra.Utils;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class EnemyBoss : Enemy<BossEnemyData>
    {
        [SerializeField] private BossCorpse _bossCorpsePrefab;
        [SerializeField] private SphereCollider _leftAttackCollider;
        [SerializeField] private SphereCollider _rightAttackCollider;
        [SerializeField] private Transform _leftAcidPoolSpawnPoint;
        [SerializeField] private Transform _rightAcidPoolSpawnPoint;
        [SerializeField] private Transform _leftFirePoint;
        [SerializeField] private Transform _rightFirePoint;

        [Foldout("SFX")][SerializeField] private AudioClip _spitAttackSFX;
        [Foldout("SFX")][SerializeField] private AudioClip _pumpAttackSFX;
        
        [SerializeField, Expandable] private BossEnemyData _data;
        
        [SerializeField] LayerMask _groundObjectLayerMask;

        private CountdownTimer _pumpAttackCooldownTimer;
        private CountdownTimer _spitAttackCooldownTimer;
        protected override BossEnemyData Data => _data;

        private AcidPool _instantiadedAcidPool;
        [Foldout("Debug"), ReadOnly][SerializeField] private bool _isIdle = true;
        [Foldout("Debug"), ReadOnly][SerializeField]private bool _isInPrePump = false;
        [Foldout("Debug"), ReadOnly][SerializeField]private bool _isInPostPump = false;
        [Foldout("Debug"), ReadOnly][SerializeField]private bool _isPerformingMeleeAttack = false;
        [Foldout("Debug"), ReadOnly][SerializeField]private bool _canSpit;
        [Foldout("Debug"), ReadOnly][SerializeField]private bool _canPump;
        [Foldout("Debug"), ReadOnly][SerializeField]private int _currentPumpCycle = 0;
        public bool IsInPrePump => _isInPrePump;

        
        private OnBossDamagedEvent _onBossDamaged;
        
        protected override void SetupStates()
        {
            var pumpAttackState = new BossPumpAttack(this, _agent, _animator);
            var spitAttackState = new BossAcidSpitAttack(this, _agent, _animator);
            var normalAttackState = new BossNormalAttack(this, _agent, _animator, Data.dashModifier);
            var idleState = new BossIdleState(this, _agent, _animator);
            var chaseState = new EnemyChaseState(this, _agent, _animator, PlayerManager.Instance.transform);
            var postPumpAttackState = new BossPostPumpAttack(this, _agent, _animator);
            
            stateMachine.AddTransition(idleState, chaseState, new FuncPredicate(CanMove));
            
            stateMachine.AddTransition(chaseState, spitAttackState, new FuncPredicate(CanPerformSpitAttack));
            stateMachine.AddTransition(chaseState, pumpAttackState, new FuncPredicate(CanPerformPumpAttack));
            stateMachine.AddTransition(chaseState, normalAttackState, new FuncPredicate(CanAttackPlayer));

            stateMachine.AddTransition(normalAttackState, chaseState, new FuncPredicate(() => !CanAttackPlayer()));
            stateMachine.AddTransition(spitAttackState, chaseState, new FuncPredicate(() => !CanPerformSpitAttack()));

            stateMachine.AddTransition(pumpAttackState, postPumpAttackState, new FuncPredicate(() => !CanPerformPumpAttack()));
            stateMachine.AddTransition(postPumpAttackState, chaseState, new FuncPredicate(() => _isInPostPump == false));
            
            
            _pumpAttackCooldownTimer = new CountdownTimer(Data.pumpAttackCooldown);
            _spitAttackCooldownTimer = new CountdownTimer(Data.spitAttackCooldown);
            
            _pumpAttackCooldownTimer.OnTimerStop += OnPumpAttackCooldownTimerFinished;
            _spitAttackCooldownTimer.OnTimerStop += OnSpitAttackCooldownTimerFinished;
            HealthController.OnDamaged += OnDamaged;

            _onBossDamaged = new OnBossDamagedEvent();
            
            stateMachine.SetState(idleState);
        }

        public override void AttachListeners()
        {
            base.AttachListeners();
            EventsAPI.Register<OnBossStartedMovingEvent>(OnBossStartMovingEvent);
        }
        
        private void OnDamaged(int value)
        {
            _onBossDamaged.damage = value;
            _onBossDamaged.normalizedDamage = HealthController.NormalizedCurrentHealth;
            
            EventsAPI.Invoke(ref _onBossDamaged);
        }
        private void OnBossStartMovingEvent(ref OnBossStartedMovingEvent moveEvent)
        {
            _isIdle = false;
                        
            _spitAttackCooldownTimer.Start();
            _pumpAttackCooldownTimer.Start();
        }

        /// <summary>
        ///     When the attack cooldown has finished, start the attackDuration timer
        /// </summary>
        private void OnPumpAttackCooldownTimerFinished()
        {
            _canPump = true;
        }
        private void OnSpitAttackCooldownTimerFinished()
        {
            _canSpit = true;
        }
        protected override void InternalUpdate()
        {
            base.InternalUpdate();
            _pumpAttackCooldownTimer?.Tick(Time.deltaTime);            
            _spitAttackCooldownTimer?.Tick(Time.deltaTime);
        }
        
        
        protected override bool CanAttackPlayer()
        {
            if (_isPerformingMeleeAttack)
            {
                return true;
            }
            //Check for special attacks and for distance to player
            return !_pumpAttackCooldownTimer.IsFinished && !_spitAttackCooldownTimer.IsFinished &&
                   IsInRangeForNormalAttack();
        }

        private bool CanPerformSpitAttack()
        {
            return _canSpit;
        }

        private bool CanPerformPumpAttack()
        {
            return _canPump;
        }
        
        private bool CanMove() => _isIdle == false;

        public void MeleeAttackStarted()
        {
            _isPerformingMeleeAttack = true;
        }
        public override void AttemptAttack()
        {
            Vector3 dir = (PlayerManager.Instance.transform.position - transform.position).normalized;
            
            UpdateFacingDirection(dir);

            SphereCollider attackCollider = CurrentDirection == FacingDirection.Left ? _leftAttackCollider : _rightAttackCollider;
            var targets = ComponentProvider.GetTargetsInSphere<IDamageable>(
                attackCollider.transform.position, attackCollider.radius, ComponentProvider.EnemyTargetsMask);
            
            
            CombatManager.Instance.PerformAttack(this, targets, baseDamage: _enemyStats.baseStrength);
            
            if(_deafultAttackSFX) AudioManager.Instance?.PlaySFXAtSource(_deafultAttackSFX, _audioSource);
            if(Data.normalAttackParticles) VFXController.SpawnParticleInWorld(Data.normalAttackParticles, attackCollider.transform.position, Quaternion.identity, destroyDuration: 1f);
            
            EventsAPI.Invoke<OnBossPerformedNormalAttack>();
        }
        
        [UsedImplicitly]
        public void OnMeleeAttackEnded()
        {
            _isPerformingMeleeAttack = false;
        }

        public void RangeAttackStart()
        {
            if(_spitAttackSFX) AudioManager.Instance?.PlaySFXAtSource(_spitAttackSFX, _audioSource);
        }
 
        [UsedImplicitly]
        public void OnRangeAttackPerformed()
        {
            
            Transform firePoint = CurrentDirection == FacingDirection.Left ? _leftFirePoint : _rightFirePoint;

            Vector3 dir = GetNormalisedDirectionToPlayer(firePoint);
            float randomDirOffsetX;
            float randomDirOffsetZ;
            float randomSpeedModifier;
            
            for (int i = 0; i < Data.howManyProjectiles; i++)
            {
                randomDirOffsetX = Random.Range(Data.directionOffSetRange.x, Data.directionOffSetRange.y);
                randomDirOffsetZ = Random.Range(Data.directionOffSetRange.x, Data.directionOffSetRange.y);
                randomSpeedModifier = Random.Range(Data.speedOffsetRange.x, Data.speedOffsetRange.y);
                Vector3 finalDir = new Vector3(dir.x + randomDirOffsetX, dir.y, dir.z + randomDirOffsetZ);
                ProjectileFactory.CreateProjectile(Data.enemyBulletData, firePoint.position, finalDir, this, randomSpeedModifier);
            }
        }
        
        [UsedImplicitly]
        public void OnRangeAttackEnded()
        {
            _canSpit = false;
            _spitAttackCooldownTimer.Restart();
        }
        
        public void PrePumpAttackStart()
        {
            _isInPrePump = true;
        }

        
        [UsedImplicitly]
        public void OnPrePumpEnded()
        {
            _isInPrePump = false;
            if(_pumpAttackSFX) AudioManager.Instance?.PlaySFXAtSource(_pumpAttackSFX, _audioSource);
        }
        
        [UsedImplicitly]
        public void OnPumpAttackPerformed()
        {
            if (_instantiadedAcidPool == null)
            {
                SpawnAcidPool();
            }
            else
            {
                PerformPumpCycle();
            }
        }
        
        
        [UsedImplicitly]
        public void OnPostPumpEnded()
        {
            _isInPostPump = false;
        }
        

        private void PerformPumpCycle()
        {
            if (_currentPumpCycle >= Data.pumpCyclesToPerform)
            {
                OnPumpAttackEnd();
                return;
            }
            
            _currentPumpCycle++;
            
            _instantiadedAcidPool.ResetDeathTimer();
            Vector3 newAcidPoolScale = _instantiadedAcidPool.transform.localScale + Data.onPumpScaleAdd;
            _instantiadedAcidPool.DoScale(newAcidPoolScale, 0.5f);
            if(_pumpAttackSFX) AudioManager.Instance?.PlaySFXAtSource(_pumpAttackSFX, _audioSource);
        }

        private void SpawnAcidPool()
        {
            Transform acidPoolSpawnPoint = CurrentDirection == FacingDirection.Left ?
                _leftAcidPoolSpawnPoint : _rightAcidPoolSpawnPoint;
            _instantiadedAcidPool = Instantiate(Data.acidPoolPrefab, 
                acidPoolSpawnPoint.position, Data.acidPoolPrefab.transform.rotation);
            _instantiadedAcidPool.Init(Data.acidLifeDurationPerCycle, Data.acidDamage);
            
            if(_pumpAttackSFX) AudioManager.Instance?.PlaySFXAtSource(_pumpAttackSFX, _audioSource);
        }

        private void OnPumpAttackEnd()
        {
            _canPump = false;
            _instantiadedAcidPool = null;
            _currentPumpCycle = 0;
            _pumpAttackCooldownTimer.Restart();

            _isInPostPump = true;
        }
        protected override void SpawnLootOnDeath()
        {
            //Do not spawn anything
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            
            EventsAPI.Invoke<OnBossDiedEvent>();
            
            Instantiate(_bossCorpsePrefab, transform.position, Quaternion.identity).Init(CurrentDirection);
        }

        public override void DetachListeners()
        {
            base.DetachListeners();
            
            EventsAPI.Unregister<OnBossStartedMovingEvent>(OnBossStartMovingEvent);

        }

        protected override void CleanUp()
        {
            base.CleanUp();
            HealthController.OnDamaged -= OnDamaged;
            _pumpAttackCooldownTimer.OnTimerStop -= OnPumpAttackCooldownTimerFinished;
            _spitAttackCooldownTimer.OnTimerStop -= OnSpitAttackCooldownTimerFinished;
        }
    }
}
