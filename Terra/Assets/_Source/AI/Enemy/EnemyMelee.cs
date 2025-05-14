using AI.Data.Definitions;
using NaughtyAttributes;
using Terra.AI.EnemyStates;
using Terra.Combat;
using Terra.FSM;
using Terra.Player;
using Terra.Utils;
using UnityEngine;

namespace Terra.AI.Enemies
{
    public class EnemyMelee : Enemy<MeleeEnemyData> 
    {
        [SerializeField, Expandable] MeleeEnemyData _data;
        protected override MeleeEnemyData Data => _data;

        protected override float GetAttackCooldown() => Data.attackCooldown;


        protected override void SetupStates()
        {
            var wander = new EnemyWanderState(this, agent, animator, Data.detectionRadius);
            var chase = new EnemyChaseState(this, agent, animator, PlayerManager.Instance.transform);
            var attack = new EnemyAttackState(this, agent, animator, PlayerManager.Instance.PlayerEntity);

            stateMachine.AddTransition(wander, chase, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            stateMachine.AddTransition(chase, wander, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            stateMachine.AddTransition(chase, attack, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
            stateMachine.AddTransition(attack, chase, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
     
            stateMachine.SetState(wander);
        }

        public override void AttemptAttack()
        {
            if (!attackTimer.IsFinished) return;

            var targets = ComponentProvider.GetTargetsInSphere<IDamageable>(
                transform.position, Data.attackRadius, ComponentProvider.EnemyTargetsMask);

            CombatManager.Instance.EnemyPerformedAttack(this, targets, enemyStats.baseStrength);
            attackTimer.Reset();
        }


    }
}