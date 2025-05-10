using Terra.AI.States.EnemyStates;
using Terra.Player;
using Terra.StateMachine;
using Terra.Utils;
using UnityEngine;

public class Enemy : EnemyBase
{
    [Header("Melee Settings")]
    [SerializeField] private float attackRadius = 1.5f;
    [SerializeField] private float timeBetweenAttacks = 1f;

    protected override float GetAttackCooldown() => timeBetweenAttacks;

    protected override void SetupStates()
    {
        var wander = new EnemyWanderState(this, animator, agent, detectionRadius);
        var chase = new EnemyChaseState(this, animator, agent, PlayerManager.Instance.transform);
        var attack = new EnemyAttackState(this, animator, agent, PlayerManager.Instance.transform);
        enemyDeathState = new EnemyDeathState(this, animator);

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

        var targets = ComponentProvider.GetTargetsInSphere<IDamagable>(
            transform.position, attackRadius, ComponentProvider.EnemyTargetsMask);

        CombatManager.Instance.EnemyPerformedAttack(targets, enemyStats.baseStrength);
        attackTimer.Reset();
    }
}