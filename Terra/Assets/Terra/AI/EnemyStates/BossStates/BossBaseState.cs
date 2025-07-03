using Terra.AI.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates.BossStates
{
    public class BossBaseState : EnemyBaseState
    {
        protected EnemyBoss boss;
        protected int rangeAttackLeft = Animator.StringToHash("RangeAttackLeft");
        protected int rangeAttackRight = Animator.StringToHash("RangeAttackRight");
        protected int prePumpAttackLeft = Animator.StringToHash("PrePumpAttackLeft");
        protected int prePumpAttackRight = Animator.StringToHash("PrePumpAttackRight");
        protected int pumpAttackLeft = Animator.StringToHash("PumpAttackLeft");
        protected int pumpAttackRight = Animator.StringToHash("PumpAttackRight");
        protected int postPumpAttackLeft = Animator.StringToHash("PostPumpAttackLeft");
        protected int postPumpAttackRight = Animator.StringToHash("PostPumpAttackRight");
        public BossBaseState(EnemyBase enemy, NavMeshAgent navMeshAgent, Animator animator) : base(enemy, navMeshAgent, animator)
        {
            if (base.enemy is EnemyBoss foundBoss)
            {
                boss = foundBoss;
            }
            else
            {
                Debug.LogError($"{enemy} is not a {nameof(EnemyBoss)}");
            }
        }
    }
}
