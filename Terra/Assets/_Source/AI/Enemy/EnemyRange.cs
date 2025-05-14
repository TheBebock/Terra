using AI.Data.Definitions;
using Terra.AI.EnemyStates;
using Terra.AI.States.EnemyStates;
using Terra.FSM;
using Terra.Player;
using UnityEngine;

namespace Terra.AI.Enemies
{
    public class EnemyRange : EnemyBase
    {
        [Header("References")]
        [SerializeField] private Transform firePoint;

        private RangedEnemyData RangedData => (RangedEnemyData)EnemyData;

        protected override float GetAttackCooldown() => RangedData.attackCooldown;

        protected override void SetupStates()
        {
            var wander = new EnemyWanderState(this, agent, animator, RangedData.detectionRadius);
            var chase = new EnemyChaseState(this, agent, animator, PlayerManager.Instance.transform);
            var attack = new EnemyRangeAttackState(this, agent, animator, PlayerManager.Instance.PlayerEntity);
            enemyDeathState = new EnemyDeathState(this, agent, animator);

            stateMachine.AddTransition(wander, chase, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            stateMachine.AddTransition(chase, wander, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            stateMachine.AddTransition(chase, attack, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
            stateMachine.AddTransition(attack, chase, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
            stateMachine.AddAnyTransition(enemyDeathState, new FuncPredicate(() => isDead));
            stateMachine.SetState(wander);
        }

        public override void AttemptAttack()
        {
            if (!attackTimer.IsFinished) return;

            if (RangedData.bulletFactory == null || firePoint == null)
            {
                Debug.LogError("EnemyRange.AttemptAttack failed: factory or firePoint missing.");
                return;
            }

            var dir = (PlayerManager.Instance.transform.position - firePoint.position).normalized;
            RangedData.bulletFactory.CreateBullet(RangedData.bulletData, firePoint.position, dir);
            attackTimer.Reset();
        }
    }
}
