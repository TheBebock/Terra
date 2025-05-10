using Terra.StateMachine;
using UnityEngine;
using UnityEngine.AI;

// Daj tutaj odpowiednią przestrzeń nazw

public class EnemyRangeAttackState : IState
{
    private readonly EnemyBase enemy;
    private readonly Animator animator;
    private readonly NavMeshAgent agent;
    private readonly Transform player;

    public EnemyRangeAttackState(EnemyBase enemy, Animator animator, NavMeshAgent agent, Transform player)
    {
        this.enemy = enemy;
        this.animator = animator;
        this.agent = agent;
        this.player = player;
    }

    public void OnEnter() => agent.isStopped = true;
    public void OnExit() => agent.isStopped = false;

    public void Tick()
    {
        if (player == null) return;
        var dir = (player.position - enemy.transform.position).normalized;
        enemy.CurrentDirection = dir.x > 0 ? EnemyBase.FacingDirection.Right : EnemyBase.FacingDirection.Left;
        enemy.AttemptAttack();
    }

    public void Update() => Tick();
    public void FixedUpdate() { }
    public void FixedTick() { }
}