using System;
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
        
        public override float AttackRange => Data.attackRange;

        protected override void SetupStates()
        {
            var wander = new EnemyWanderState(this, agent, animator, Data.attackRange);
            var chase = new EnemyChaseState(this, agent, animator, PlayerManager.Instance.transform);
            var attack = new EnemyAttackState(this, agent, animator, PlayerManager.Instance.PlayerEntity);
            

            stateMachine.AddTransition(chase, attack, new FuncPredicate(() => playerDetector.CanAttackPlayer() && Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) <= AttackRange));
            stateMachine.AddTransition(attack, chase, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
     
            stateMachine.AddAnyTransition(wander, new FuncPredicate(()=>PlayerManager.Instance.IsPlayerDead));
            stateMachine.SetState(chase);
        }

        public override void AttemptAttack()
        {
            Vector3 dir = (PlayerManager.Instance.transform.position - transform.position).normalized;
            
            UpdateFacingDirection(dir);
            
            var targets = ComponentProvider.GetTargetsInSphere<IDamageable>(
                transform.position, Data.attackRadius, ComponentProvider.EnemyTargetsMask);

            CombatManager.Instance.PerformAttack(this, targets, baseDamage: enemyStats.baseStrength);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawWireSphere(transform.position, Data.attackRadius);
        }
    }
}