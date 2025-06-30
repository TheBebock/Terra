using NaughtyAttributes;
using Terra.AI.Data;
using Terra.AI.EnemyStates;
using Terra.FSM;
using Terra.Player;
using Terra.Utils;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class EnemyTank : Enemy<TankEnemyData>
    {
        //TODO:Either collider or raycast
        [SerializeField] private Collider _attackCollider;
        [SerializeField, Expandable] private TankEnemyData _data;
        protected override TankEnemyData Data => _data;

        private CountdownTimer _attackCooldownTimer;
        private CountdownTimer _attackDurationTimer;
        protected override void SetupStates()
        {
            var wander = new EnemyWanderState(this, _agent, _animator);            
            var walkAndAttack = new EnemyWalkAndAttackState(this, _agent, _animator, PlayerManager.Instance.PlayerEntity);            
            var tired = new EnemyTankTiredState(this, _agent, _animator);

            _attackCooldownTimer = new CountdownTimer(Data.attackCooldown);
            _attackDurationTimer = new CountdownTimer(Data.attackDuration);

            _attackDurationTimer.OnTimerStop += OnAttackDurationTimerFinished;
            _attackCooldownTimer.OnTimerStop += OnAttackCooldownTimerFinished;
            
            _attackCooldownTimer.Start();
            
            stateMachine.AddTransition(walkAndAttack, tired, new FuncPredicate(CanEnterTiredState));
            stateMachine.AddTransition(tired, walkAndAttack, new FuncPredicate(CanAttackPlayer));

            stateMachine.AddAnyTransition(wander, new FuncPredicate(()=>PlayerManager.Instance.IsPlayerDead && !isDead));
            stateMachine.SetState(tired);
        }

        public void RestartAttackCooldown()
        {
            _attackCooldownTimer?.Restart();
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
        
        public override void AttemptAttack()
        {
            //TODO: Either raycast or enable collider
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            _attackDurationTimer.OnTimerStop -= OnAttackDurationTimerFinished;
            _attackCooldownTimer.OnTimerStop -= OnAttackCooldownTimerFinished;
        }
    }
}
