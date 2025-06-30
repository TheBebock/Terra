using Terra.AI.Enemy;
using Terra.Enums;
using Terra.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates
{
    public class EnemyWalkAndAttackState : EnemyBaseAttackState
    {
        private int _animationName;
        private EnemyTank _tank;
        public EnemyWalkAndAttackState(EnemyBase enemy, NavMeshAgent agent, Animator animator, PlayerEntity player) : base(enemy, agent, animator, player)
        {
            if (enemy is EnemyTank tank)
            {
                _tank = tank;
            }
            else
            {
                Debug.LogError($"{enemy.name} is not a {nameof(EnemyTank)}, but has {nameof(EnemyTankTiredState)} assigned");
            }
        }

        public override void OnEnter() 
        {
            //Noop
        }

        public override void Update()
        {
            base.Update();
         
            //TODO: Set nav mesh agent path, decide whether to dynamically adjust Left/Right facing dir, decide whether to follow player or get few random points
            int temp = enemy.CurrentDirection == FacingDirection.Left ? AnimationHashes.WalkLeft : AnimationHashes.WalkRight;
            if(temp == _animationName) return;
            _animationName = temp;
            animator.CrossFade(_animationName, CrossFadeDuration);
            
        }

        protected override void OnAttack()
        {
            _tank.AttemptAttack();
        }
    }
}
