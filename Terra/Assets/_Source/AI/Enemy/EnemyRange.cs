using _Source.AI.Enemy;
using _Source.AI.EnemyStates;
using AI.Data.Definitions;
using NaughtyAttributes;
using Terra.AI.States.EnemyStates;
using Terra.FSM;
using Terra.Player;
using UnityEngine;

namespace Terra.AI.Enemies
{
    public class EnemyRange : Enemy<RangedEnemyData>
    {
        [SerializeField, Expandable] RangedEnemyData _data;
        protected override RangedEnemyData Data => _data;
        
        [Header("References")]
        [SerializeField] private Transform firePoint;
        

        protected override float GetAttackCooldown() => Data.attackCooldown;

        protected override void SetupStates()
        {
            var wander = new EnemyWanderState(this, agent, animator, Data.detectionRadius);
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

            if (Data.bulletFactory == null || firePoint == null)
            {
                Debug.LogError("EnemyRange.AttemptAttack failed: factory or firePoint missing.");
                return;
            }

            var dir = (PlayerManager.Instance.transform.position - firePoint.position).normalized;
            Data.bulletFactory.CreateBullet(Data.bulletData, firePoint.position, dir);
            attackTimer.Reset();
        }


    }
}
