using Terra.AI.States.EnemyStates;
using Terra.Player;
using Terra.StateMachine;
using UnityEngine;

public class EnemyRange : EnemyBase
{
    [Header("Ranged Settings")]
    [SerializeField] private BulletFactory bulletFactory;
    [SerializeField] private Transform firePoint;
    [SerializeField] private BulletData bulletData;
    [SerializeField] private float attackCooldown = 1.5f;

    protected override float GetAttackCooldown() => attackCooldown;

    protected override void SetupStates()
    {
        var wander = new EnemyWanderState(this, animator, agent, detectionRadius);
        var chase = new EnemyChaseState(this, animator, agent, PlayerManager.Instance.transform);
        var attack = new EnemyRangeAttackState(this, animator, agent, PlayerManager.Instance.transform);
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

        if (bulletFactory == null || firePoint == null)
        {
            Debug.LogError("EnemyRange.AttemptAttack failed: factory or firePoint missing.");
            return;
        }

        var dir = (PlayerManager.Instance.transform.position - firePoint.position).normalized;
        bulletFactory.CreateBullet(bulletData, firePoint.position, dir);
        attackTimer.Reset();
    }
}