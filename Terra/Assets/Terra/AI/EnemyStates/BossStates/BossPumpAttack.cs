using Terra.AI.Enemy;
using Terra.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates.BossStates
{
    public class BossPumpAttack : BossBaseState
    {
        FacingDirection _pumpDirection;
        private int _animationName;
        public BossPumpAttack(EnemyBase enemy, NavMeshAgent navMeshAgent, Animator animator) : base(enemy, navMeshAgent, animator)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
            
            boss.UpdateFacingDirection(boss.GetNormalisedDirectionToPlayer());
            boss.PrePumpAttackStart();
            _pumpDirection = enemy.CurrentDirection;
            _animationName = _pumpDirection == FacingDirection.Left ? prePumpAttackLeft : pumpAttackRight;
            animator.CrossFade(_animationName, CrossFadeDuration);
        }

        public override void Update()
        {
            base.Update();
            
            if(boss.IsInPrePump) return;
            int previousAnimationName = _animationName;
            _animationName = _pumpDirection == FacingDirection.Left ? pumpAttackLeft : pumpAttackRight;
            if(previousAnimationName == _animationName) return;
            
            animator.CrossFade(_animationName, CrossFadeDuration);
        }
        
    }
}
