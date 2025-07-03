using Terra.AI.Enemy;
using Terra.Enums;
using Terra.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates {
    public class EnemyMeleeAttackState : EnemyBaseAttackState
    {

        private float _attackDashModifier = 1f;
        public EnemyMeleeAttackState(EnemyBase enemy, NavMeshAgent agent, Animator animator, PlayerEntity player) : base(enemy, agent, animator, player)
        {
            if (enemy is EnemyMelee melee)
            {
                _attackDashModifier = melee.AttackDashModifier;
            }
            else
            {
                Debug.LogWarning($"{enemy.name} is not a {nameof(EnemyMelee)}, but has {nameof(EnemyMeleeAttackState)} assigned");
            }
        }
        
        public EnemyMeleeAttackState(EnemyBase enemy, NavMeshAgent agent, Animator animator, PlayerEntity player, 
            float attackDashModifier) : base(enemy, agent, animator, player)
        {
            _attackDashModifier = attackDashModifier;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            navMeshAgent.velocity *= _attackDashModifier;
            
            Vector3 dir = (PlayerManager.Instance.transform.position - enemy.transform.position).normalized;
            enemy.UpdateFacingDirection(dir);
            int animationName = enemy.CurrentDirection == FacingDirection.Left ? AnimationHashes.AttackLeft : AnimationHashes.AttackRight;
            animator.CrossFade(animationName, CrossFadeDuration);
        }

        protected override void OnAttack()
        {
        }
    }
}