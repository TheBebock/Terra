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
            

            StateMachine.AddTransition(chase, attack, new FuncPredicate(() => playerDetector.CanAttackPlayer() && Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) <= AttackRange));
            StateMachine.AddTransition(attack, chase, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
     
            StateMachine.AddAnyTransition(wander, new FuncPredicate(()=>PlayerManager.Instance.IsPlayerDead));
            StateMachine.SetState(chase);
        }

        public override void AttemptAttack()
        {
            if (!AttackTimer.IsFinished) return;

            var targets = ComponentProvider.GetTargetsInSphere<IDamageable>(
                transform.position, Data.attackRadius, ComponentProvider.EnemyTargetsMask);

            CombatManager.Instance.PerformAttack(this, targets, baseDamage: enemyStats.baseStrength);
            AttackTimer.Reset();
        }


    }
}