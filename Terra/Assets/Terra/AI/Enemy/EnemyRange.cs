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
            enemyDeathState = new EnemyDeathState(this, agent, animator);


            stateMachine.AddTransition(chase, attack, new FuncPredicate(() => playerDetector.CanAttackPlayer() && Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) <= AttackRange));
            stateMachine.AddTransition(attack, chase, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
            
            stateMachine.AddAnyTransition(wander, new FuncPredicate(()=>PlayerManager.Instance.IsPlayerDead));
            stateMachine.AddAnyTransition(enemyDeathState, new FuncPredicate(() => isDead));
            
            stateMachine.SetState(wander);
        }

        public override void AttemptAttack()
        {
            if (!attackTimer.IsFinished) return;
            if (firePoint == null) { Debug.LogError("firePoint missing"); return; }


            Vector3 dir = (PlayerManager.Instance.transform.position - firePoint.position).normalized;

            Quaternion rot = Quaternion.LookRotation(dir);

   
            Projectile p = ProjectileFactory.CreateProjectile(
                Data.bulletData,
                firePoint.position,
                dir,
                this
            );

            p.transform.rotation = rot;

            attackTimer.Reset();
        }
    }
}
