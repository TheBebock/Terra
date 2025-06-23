using NaughtyAttributes;
using Terra.AI.Data.Definitions;
using Terra.AI.EnemyStates;
using Terra.Combat;
using Terra.FSM;
using Terra.Managers;
using Terra.Player;
using Terra.Utils;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class EnemyMelee : Enemy<MeleeEnemyData> 
    {
        [SerializeField, Expandable] private MeleeEnemyData _data;
        protected override MeleeEnemyData Data => _data;

        protected override float GetAttackCooldown() => Data.attackCooldown;
        

        protected override void SetupStates()
        {
            var wander = new EnemyWanderState(this, _agent, _animator, Data.attackRange);
            var chase = new EnemyChaseState(this, _agent, _animator, PlayerManager.Instance.transform);
            var attack = new EnemyAttackState(this, _agent, _animator, PlayerManager.Instance.PlayerEntity);
            

            StateMachine.AddTransition(chase, attack,
                new FuncPredicate(() => _playerDetector.CanAttackPlayer() 
                                        && !IsDead));
            StateMachine.AddTransition(attack, chase, new FuncPredicate(() => !_playerDetector.CanAttackPlayer() && !IsDead));
     
            StateMachine.AddAnyTransition(wander, new FuncPredicate(()=>PlayerManager.Instance.IsPlayerDead && !IsDead));
            StateMachine.SetState(chase);
        }

        public override void AttemptAttack()
        {
            Vector3 dir = (PlayerManager.Instance.transform.position - transform.position).normalized;
            
            UpdateFacingDirection(dir);
            
            var targets = ComponentProvider.GetTargetsInSphere<IDamageable>(
                transform.position, Data.attackRadius, ComponentProvider.EnemyTargetsMask);

            CombatManager.Instance.PerformAttack(this, targets, baseDamage: _enemyStats.baseStrength);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawWireSphere(transform.position, Data.attackRadius);
        }
    }
}