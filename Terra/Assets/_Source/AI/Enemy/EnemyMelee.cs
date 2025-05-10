using Terra.AI.EnemyStates;
using Terra.Combat;
using Terra.FSM;
using Terra.Player;
using Terra.Utils;
using UnityEngine;

namespace Terra.AI.Enemies
{
    public class EnemyMelee : EnemyBase
    {
        [Header("Melee Settings")] [SerializeField]
        private float attackRadius = 1.5f;

        [SerializeField] private float timeBetweenAttacks = 1f;

        protected override float GetAttackCooldown() => timeBetweenAttacks;

        protected override void SetupStates()
        {
            var wander = new EnemyWanderState(this, agent, animator, detectionRadius);
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
                transform.position, attackRadius, ComponentProvider.EnemyTargetsMask);

            CombatManager.Instance.EnemyPerformedAttack(targets, enemyStats.baseStrength);
            attackTimer.Reset();
        }
    }
}