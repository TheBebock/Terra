using System.Collections.Generic;
using Terra.AI.Enemy;
using Terra.Enums;
using Terra.Player;
using Terra.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Terra.AI.EnemyStates
{
    public class EnemyWalkAndAttackState : EnemyBaseAttackState
    {
        private int _animationName;
        private EnemyTank _tank;
        private float _minimalDistance;
        private float _maxDistance;
        
        private List<Vector3> _points = new();
        private int _currentPoint = -1;
        private int _amountOfPoints = 10; 

        public EnemyWalkAndAttackState(EnemyBase enemy, NavMeshAgent agent, Animator animator, 
            PlayerEntity player, float maxDistance) : base(enemy, agent, animator, player)
        {
            _maxDistance = maxDistance;
            _minimalDistance = maxDistance/2;
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
            if(!_tank) return;
            
            _tank.AttackCollider.EnableCollider();

            for (int i = 0; i < _amountOfPoints; i++)
            {
                Vector3 pointPosition = i == 0 ? enemy.transform.position : _points[i-1];
                
                _points.Add(RaycastExtension.GetPositionInCircle(pointPosition, _maxDistance, 
                    _tank.GroundObjectLayerMask, _tank.TargetLayer, _minimalDistance));
            }
            
            _animationName = enemy.CurrentDirection == FacingDirection.Left ? AnimationHashes.AttackLeft : AnimationHashes.AttackRight;
            animator.CrossFade(_animationName, CrossFadeDuration);
        }

        public override void Update()
        {
            base.Update();
         
            TryUpdatingPath();
            
            int temp = enemy.CurrentDirection == FacingDirection.Left ? AnimationHashes.AttackLeft : AnimationHashes.AttackRight;
            if(temp == _animationName) return;
            _animationName = temp;
            animator.CrossFade(_animationName, CrossFadeDuration);
            
        }

        private void TryUpdatingPath()
        {
            if (navMeshAgent.pathPending) return;

            if (navMeshAgent.remainingDistance >= navMeshAgent.stoppingDistance) return;

            _currentPoint++;
            if(_currentPoint >= _points.Count) _currentPoint = 0;
            
            Vector3 nextPoint = _points[_currentPoint];
            navMeshAgent.SetDestination(nextPoint);
        }
        protected override void OnAttack()
        {
            //Noop
        }

        public override void OnExit()
        {
            _points.Clear();
            _currentPoint = -1;
            _tank?.AttackCollider.DisableCollider();
        }
    }
}
