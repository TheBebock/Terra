using NaughtyAttributes;
using Terra.AI.Data;
using Terra.Utils;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class EnemyBoss : Enemy<BossEnemyData>
    {
        [SerializeField] private EnemyColliderComponent _leftAttackCollider;
        [SerializeField] private EnemyColliderComponent _rightAttackCollider;
        [SerializeField, Expandable] private BossEnemyData _data;
        
        [SerializeField] LayerMask _groundObjectLayerMask;
        private int _targetLayer;

        private CountdownTimer _attackCooldownTimer;
        protected override BossEnemyData Data => _data;

        public LayerMask GroundObjectLayerMask => _groundObjectLayerMask;
        public int TargetLayer => _targetLayer;
        public EnemyColliderComponent LeftAttackCollider => _leftAttackCollider;
        public EnemyColliderComponent RightAttackCollider => _rightAttackCollider;

        protected override void Awake()
        {
            base.Awake();
         
            return;
            _targetLayer = LayerMask.NameToLayer("Ground");
            _leftAttackCollider.Init(this, _enemyStats.baseStrength);
            _leftAttackCollider.DisableCollider();
            _rightAttackCollider.Init(this, _enemyStats.baseStrength);
            _rightAttackCollider.DisableCollider();
        }

        protected override void SetupStates()
        {

            _attackCooldownTimer = new CountdownTimer(Data.attackCooldown);
            _attackCooldownTimer.OnTimerStop += OnAttackCooldownTimerFinished;
            
            _attackCooldownTimer.Start();
            
            
        }
        

        /// <summary>
        ///     When the attack cooldown has finished, start the attackDuration timer
        /// </summary>
        private void OnAttackCooldownTimerFinished()
        {
            
        }
        
        protected override void InternalUpdate()
        {
            base.InternalUpdate();
            _attackCooldownTimer?.Tick(Time.deltaTime);
        }
        
        protected override bool CanAttackPlayer()
        {
            return _attackCooldownTimer.IsFinished;
        }
        

        protected override void SpawnLootOnDeath()
        {
            //Nothing
        }
        
        public override void AttemptAttack()
        {
            //Noop
        }
        

        protected override void CleanUp()
        {
            base.CleanUp();
            _attackCooldownTimer.OnTimerStop -= OnAttackCooldownTimerFinished;
        }
    }
}
