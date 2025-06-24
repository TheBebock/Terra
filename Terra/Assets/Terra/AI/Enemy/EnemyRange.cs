using System;
using NaughtyAttributes;
using Terra.AI.Data.Definitions;
using Terra.AI.EnemyStates;
using Terra.Combat.Projectiles;
using Terra.FSM;
using Terra.Player;
using UnityEngine;

namespace Terra.AI.Enemy
{
    public class EnemyRange : Enemy<RangedEnemyData>
    {
        [SerializeField, Expandable] RangedEnemyData _data;
        protected override RangedEnemyData Data => _data;
        [SerializeField] float _bulletRaycastOffset = 0.2f;
        [Header("References")]
        [SerializeField] private Transform _firePoint;
        [SerializeField] LayerMask _obstructionLayerMask;

        protected override float GetAttackCooldown() => Data.attackCooldown;

        protected override void SetupStates()
        {
            var wander = new EnemyWanderState(this, _agent, _animator);
            var chase = new EnemyChaseState(this, _agent, _animator, PlayerManager.Instance.transform);
            var attack = new EnemyRangeAttackState(this, _agent, _animator, PlayerManager.Instance.PlayerEntity);
            enemyDeathState = new EnemyDeathState(this, _agent, _animator);

            stateMachine.AddTransition(wander, chase, new FuncPredicate(() => !CanAttackPlayer()));
            stateMachine.AddTransition(chase, wander, new FuncPredicate(CanAttackPlayer));
            
            stateMachine.AddTransition(wander, attack, new FuncPredicate(() => CanAttackPlayer() && !IsPlayerNear() && !CheckForObstructions()));
            stateMachine.AddTransition(attack, wander, new FuncPredicate(()=> IsPlayerNear() || CheckForObstructions()));
            
            stateMachine.AddAnyTransition(wander, new FuncPredicate(()=>PlayerManager.Instance.IsPlayerDead));
            stateMachine.AddAnyTransition(enemyDeathState, new FuncPredicate(() => isDead));
            
            stateMachine.SetState(wander);
        }

        private bool IsPlayerNear()
        {
            return Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) <= Data.keepDistanceFromPlayer;
        }

        private bool CheckForObstructions()
        {
            Vector3 direction = PlayerManager.Instance.transform.position - _firePoint.position;
            float distance = AttackRange;
            direction.Normalize();
            
            Vector3 leftRayOrigin = new(_firePoint.position.x - _bulletRaycastOffset, _firePoint.position.y, _firePoint.position.z);
            Vector3 rightRayOrigin = new(_firePoint.position.x + _bulletRaycastOffset, _firePoint.position.y, _firePoint.position.z);
            RaycastHit hit;
            
            if (Physics.Raycast(leftRayOrigin, direction, out hit, distance, _obstructionLayerMask))
            {
                Debug.LogWarning($"[Left] {hit.transform.name} is obstructing");
                return true;
            }

            // Right ray
            if (Physics.Raycast(rightRayOrigin, direction, out hit, distance, _obstructionLayerMask))
            {
                Debug.LogWarning($"[Right] {hit.transform.name} is obstructing");
                return true;
            }
            

            return false; 
        }

        public override void AttemptAttack()
        {
            if (!attackTimer.IsFinished) return;
            if (_firePoint == null) { Debug.LogError("firePoint missing"); return; }


            Vector3 dir = (PlayerManager.Instance.transform.position - _firePoint.position).normalized;

            Quaternion rot = Quaternion.LookRotation(dir);

            UpdateFacingDirection(dir);
            
            Projectile p = ProjectileFactory.CreateProjectile(
                Data.bulletData,
                _firePoint.position,
                dir,
                this
            );

            p.transform.rotation = rot;

            attackTimer.Restart();
        }

        private void OnDrawGizmos()
        {
            Vector3 direction = PlayerManager.Instance.PlayerEntity.transform.position - _firePoint.position;
            float distance = AttackRange;
            direction.Normalize();
             
            Vector3 leftRayOrigin = new(_firePoint.position.x - _bulletRaycastOffset, _firePoint.position.y, _firePoint.position.z);
            Vector3 rightRayOrigin = new(_firePoint.position.x + _bulletRaycastOffset, _firePoint.position.y, _firePoint.position.z);
            Gizmos.color = Color.yellow;
            
            Gizmos.DrawLine(leftRayOrigin,  leftRayOrigin + direction * distance);
            Gizmos.DrawLine(rightRayOrigin,  rightRayOrigin + direction * distance);
        }
    }
}
