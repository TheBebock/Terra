using _Source.AI.Data.Definitions;
using _Source.AI.EnemyStates;
using NaughtyAttributes;
using Terra.Combat;
using Terra.FSM;
using Terra.Player;
using Terra.Utils;
using UnityEngine;

namespace _Source.AI.Enemy
{
    public class EnemyMelee : Enemy<MeleeEnemyData> 
    {
        [SerializeField, Expandable] MeleeEnemyData data;
        protected override MeleeEnemyData Data => data;

        protected override float GetAttackCooldown() => Data.attackCooldown;


        public override float AttackRange => Data.attackRange;

        protected override void SetupStates()
        {
            var wander = new EnemyWanderState(this, agent, animator, Data.detectionRadius);
            var chase = new EnemyChaseState(this, agent, animator, PlayerManager.Instance.transform);
            var attack = new EnemyAttackState(this, agent, animator, PlayerManager.Instance.PlayerEntity);

            StateMachine.AddTransition(wander, chase, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            StateMachine.AddTransition(chase, wander, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            StateMachine.AddTransition(chase, attack, new FuncPredicate(() => playerDetector.CanAttackPlayer() && Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) <= AttackRange));
            StateMachine.AddTransition(attack, chase, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
     
            StateMachine.SetState(wander);
        }

        public override void AttemptAttack()
        {
            if (!AttackTimer.IsFinished) return;

            var targets = ComponentProvider.GetTargetsInSphere<IDamageable>(
                transform.position, Data.attackRadius, ComponentProvider.EnemyTargetsMask);

            CombatManager.Instance.EnemyPerformedAttack(this, targets, enemyStats.baseStrength);
            AttackTimer.Reset();
        }


    }
}