using Terra.AI.Enemies;
using Terra.AI.EnemyStates;
using Terra.Enums;
using Terra.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.States.EnemyStates
{
    public class EnemyRangeAttackState : EnemyBaseAttackState
    {
        public EnemyRangeAttackState(EnemyBase enemy, NavMeshAgent agent, Animator animator, PlayerEntity player) :
            base(enemy, agent, animator, player)
        {
        }

        protected override void OnAttack()
        {
            if (player == null)
            {
                Debug.LogError($"{this}: Player is null");
                return;
            }
            var dir = (player.transform.position - enemy.transform.position).normalized;
            //TODO: Rotate enemy and set correct animation
            enemy.CurrentDirection = dir.x > 0 ? FacingDirection.Right : FacingDirection.Left;
            enemy.AttemptAttack();
        }
    }
}