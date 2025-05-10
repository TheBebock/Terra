using Terra.AI.Enemies;
using Terra.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates
{
    public abstract class EnemyBaseAttackState : EnemyBaseState
    {
        protected readonly PlayerEntity player;

        public EnemyBaseAttackState(EnemyBase enemy, NavMeshAgent agent, Animator animator, PlayerEntity player) : base(
            enemy, agent, animator)
        {
            this.player = player;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }

        public override void Update()
        {
            base.Update();
            OnAttack();
        }

        /// <summary>
        ///     Called on Update, implements attack logic
        /// </summary>
        protected abstract void OnAttack();

        public override void OnExit()
        {
            base.OnExit();
            navMeshAgent.isStopped = false;
        }
    }
}
