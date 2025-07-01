using Terra.AI.Enemy;
using Terra.Enums;
using Terra.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates {
    public class EnemyMeleeAttackState : EnemyBaseAttackState {
        
        private EnemyMelee _melee;
        public EnemyMeleeAttackState(EnemyBase enemy, NavMeshAgent agent, Animator animator, PlayerEntity player) : base(enemy, agent, animator, player)
        {
            if (enemy is EnemyMelee melee)
            {
                _melee = melee;
            }
            else
            {
                Debug.LogError($"{enemy.name} is not a {nameof(EnemyMelee)}, but has {nameof(EnemyMeleeAttackState)} assigned");
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            navMeshAgent.velocity *= _melee.AttackDashModifier;
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