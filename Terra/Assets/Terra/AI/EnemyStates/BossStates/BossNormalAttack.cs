using Terra.AI.Enemy;
using Terra.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates.BossStates
{
    public class BossNormalAttack : BossBaseState
    {
        private int _animationName;
        private int _previousAnimName;
        public BossNormalAttack(EnemyBase enemy, NavMeshAgent navMeshAgent, Animator animator) : base(enemy, navMeshAgent, animator)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            boss.MeleeAttackStarted();
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
            
            boss.UpdateFacingDirection(boss.GetNormalisedDirectionToPlayer());
            _animationName = boss.CurrentDirection == FacingDirection.Left ? 
                AnimationHashes.AttackLeft : AnimationHashes.AttackRight;
            animator.CrossFade(_animationName, CrossFadeDuration);
        }

        public override void Update()
        {
            base.Update();
            
            boss.UpdateFacingDirection(boss.GetNormalisedDirectionToPlayer());
            _previousAnimName = _animationName;
            
            _animationName = boss.CurrentDirection == FacingDirection.Left ? 
                AnimationHashes.AttackLeft : AnimationHashes.AttackRight;

            if (_animationName != _previousAnimName)
            {
                animator.CrossFade(_animationName, CrossFadeDuration);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            
            navMeshAgent.isStopped = false;
        }
    }
}
