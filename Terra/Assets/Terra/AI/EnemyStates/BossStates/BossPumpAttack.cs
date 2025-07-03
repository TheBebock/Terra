using Terra.AI.Enemy;
using Terra.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates.BossStates
{
    public class BossPumpAttack : BossBaseState
    {
        FacingDirection pumpDirection;
        private int animationName;
        public BossPumpAttack(EnemyBase enemy, NavMeshAgent navMeshAgent, Animator animator) : base(enemy, navMeshAgent, animator)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            boss.PrePumpAttackStart();
            pumpDirection = enemy.CurrentDirection;
            animationName = pumpDirection == FacingDirection.Left ? prePumpAttackLeft : pumpAttackRight;
            animator.CrossFade(animationName, CrossFadeDuration);
        }

        public override void Update()
        {
            base.Update();
            
            if(boss.IsInPrePump) return;
            int previousAnimationName = animationName;
            animationName = pumpDirection == FacingDirection.Left ? pumpAttackLeft : pumpAttackRight;
            if(previousAnimationName == animationName) return;
            
            animator.CrossFade(animationName, CrossFadeDuration);
        }
        
    }
}
