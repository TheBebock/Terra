using NaughtyAttributes;
using Terra.AI.Data.Definitions;
using Terra.AI.EnemyStates;
using Terra.Combat.Projectiles;
using Terra.FSM;
using Terra.Player;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class EnemyRange : Enemy<RangedEnemyData>
    {
        [SerializeField, Expandable] RangedEnemyData _data;
        protected override RangedEnemyData Data => _data;
        
        [Header("References")]
        [SerializeField] private Transform firePoint;
        

        protected override float GetAttackCooldown() => Data.attackCooldown;

        public override float AttackRange => Data.attackRange;

        protected override void SetupStates()
        {
            var wander = new EnemyWanderState(this, agent, animator, Data.detectionRadius);
            var chase = new EnemyChaseState(this, agent, animator, PlayerManager.Instance.transform);
            var attack = new EnemyRangeAttackState(this, agent, animator, PlayerManager.Instance.PlayerEntity);
            EnemyDeathState = new EnemyDeathState(this, agent, animator);

            StateMachine.AddTransition(wander, chase, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            StateMachine.AddTransition(chase, wander, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            StateMachine.AddTransition(chase, attack, new FuncPredicate(() => playerDetector.CanAttackPlayer() && Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) > AttackRange));
            StateMachine.AddTransition(attack, chase, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
            StateMachine.AddAnyTransition(EnemyDeathState, new FuncPredicate(() => IsDead));
            StateMachine.SetState(wander);
        }

        public override void AttemptAttack()
        {
            if (!AttackTimer.IsFinished) return;

            if (firePoint == null)
            {
                Debug.LogError("EnemyRange.AttemptAttack failed: firePoint missing.");
                return;
            }

            var dir = (PlayerManager.Instance.transform.position - firePoint.position).normalized;
            ProjectileFactory.CreateProjectile(Data.bulletData, firePoint.position, dir, this);
            AttackTimer.Reset();
        }
    }
}
