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
        [SerializeField] private Transform _firePoint;
        

        protected override float GetAttackCooldown() => Data.attackCooldown;

        protected override void SetupStates()
        {
            var wander = new EnemyWanderState(this, _agent, _animator, Data.detectionRadius);
            var chase = new EnemyChaseState(this, _agent, _animator, PlayerManager.Instance.transform);
            var attack = new EnemyRangeAttackState(this, _agent, _animator, PlayerManager.Instance.PlayerEntity);
            EnemyDeathState = new EnemyDeathState(this, _agent, _animator);


            StateMachine.AddTransition(wander, attack, new FuncPredicate(() => _playerDetector.CanAttackPlayer() && Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) <= AttackRange && !IsPlayerNear()));
            StateMachine.AddTransition(attack, wander, new FuncPredicate(() => !_playerDetector.CanAttackPlayer() || IsPlayerNear()));
            
            StateMachine.AddAnyTransition(wander, new FuncPredicate(()=>PlayerManager.Instance.IsPlayerDead));
            StateMachine.AddAnyTransition(EnemyDeathState, new FuncPredicate(() => IsDead));
            
            StateMachine.SetState(wander);
        }

        private bool IsPlayerNear()
        {
            return Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) <= Data.detectionRadius;
        }

        public override void AttemptAttack()
        {
            if (!AttackTimer.IsFinished) return;
            if (_firePoint == null) { Debug.LogError("firePoint missing"); return; }


            Vector3 dir = (PlayerManager.Instance.transform.position - _firePoint.position).normalized;

            Quaternion rot = Quaternion.LookRotation(dir);

            UpdateFacingDirection(dir);
            
            Projectile p = ProjectileFactory.CreateProjectile(
                Data.bulletData,
                _firePoint.position,
                dir,
                this
            );

            p.transform.rotation = rot;

            AttackTimer.Restart();
        }
    }
}
