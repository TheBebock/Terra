using System;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Terra.Interfaces;
using Terra.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using static Terra.Player.PlayerMovement;

namespace Terra.Player
{

    /// <summary>
    /// Handles player attacks
    /// </summary>
    [Serializable]
    public class PlayerAttackController : IAttachListeners
    {
        private InputSystem.PlayerControlsActions inputActions;
        [Foldout("Debug"), ReadOnly] [SerializeField]
        
        private PlayerManager _playerManager;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private bool isTryingPerformMeleeAttack = false;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private bool isTryingPerformDistanceAttack = false;


        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float currentMeleeCD = 0;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float currentRangedCD = 0;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float maxMeleeCD = 0;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float maxRangedCD = 0;

        public bool IsTryingPerformMeleeAttack
        {
            get => isTryingPerformMeleeAttack;
            set => isTryingPerformMeleeAttack = value;

        }

        public bool IsTryingPerformDistanceAttack
        {
            get => isTryingPerformDistanceAttack;
            set => isTryingPerformDistanceAttack = value;
        }

        public float CurrentMeleeCooldown => currentMeleeCD;
        public float CurrentRangedCooldown => currentRangedCD;

        private PlayerAttackDirection currentplayerAttackDirection;
        public PlayerAttackDirection CurrentPlayerAttackDirection => currentplayerAttackDirection;

        public enum PlayerAttackDirection
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3,
        }

        public PlayerAttackController(InputSystem.PlayerControlsActions playerControls, PlayerManager playerManager)
        {
            inputActions = playerControls;
            _playerManager = playerManager;

            AttachListeners();
        }

        public void AttachListeners()
        {
            inputActions.MelleAttack.performed += OnMeleeAttackInput;
            inputActions.DistanceAttack.performed += OnDistanceAttackInput;
        }

        public IEnumerator DecreaseMeleeCooldown()
        {
            while (CurrentMeleeCooldown > 0)
            {
                currentMeleeCD--;
                yield return new WaitForSeconds(0.1f);
            }
        }

        public IEnumerator DecreaseRangedCooldown()
        {
            while (CurrentRangedCooldown > 0)
            {
                currentRangedCD--;
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnMeleeAttackInput(InputAction.CallbackContext context)
        {
            maxMeleeCD = _playerManager.PlayerInventory.MeleeWeapon.Data.attackSpeed;
            if (currentMeleeCD == 0)
            {
                ChangeAttackDirection();
                isTryingPerformMeleeAttack = true;
                currentMeleeCD = maxMeleeCD;
                _playerManager.StartCoroutine(DecreaseMeleeCooldown());
            }
        }

        private void OnDistanceAttackInput(InputAction.CallbackContext context)
        {
            //TODO: Ranged attack
            return;
            
            maxRangedCD = _playerManager.PlayerInventory.RangedWeapon.Data.attackSpeed;
            if (currentRangedCD == 0)
            {
                ChangeAttackDirection();
                isTryingPerformDistanceAttack = true;
                currentRangedCD = maxRangedCD;
                _playerManager.StartCoroutine(DecreaseRangedCooldown());
            }
        }

        private void ChangeAttackDirection()
        {
            if(isTryingPerformMeleeAttack) return;
            
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay( mousePosition );
            Plane plane = new Plane(Vector3.up, _playerManager.transform.position);

            // Raycast get point where player clicked on screen while we use perspective camera
            if (!plane.Raycast(ray, out float enter)) return;
            
            
            Vector3 worldClickPosition = ray.GetPoint(enter);
            Vector3 direction = (worldClickPosition - _playerManager.transform.position).normalized;

            if(Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                if(direction.x > 0) currentplayerAttackDirection = PlayerAttackDirection.Right;
                else currentplayerAttackDirection = PlayerAttackDirection.Left;
            }
            else
            {
                if(direction.z > 0) currentplayerAttackDirection = PlayerAttackDirection.Up;
                else currentplayerAttackDirection = PlayerAttackDirection.Down;
            }
            
            //TODO: Refactor later - move to attack state

            float range = _playerManager.PlayerInventory.MeleeWeapon.Data.range;
            
            Vector3 attackPosition = _playerManager.transform.position + direction * range;
            
            Debug.Log("Attack position" + attackPosition);
            List<IDamagable> targets = ContactProvider.GetTargetsInSphere<IDamagable>(attackPosition, range, ContactProvider.PlayerTargetsMask);

            CombatManager.Instance.PlayerPerformedAttack(targets, _playerManager.PlayerInventory.MeleeWeapon.Data.damage);

        }


        public void DetachListeners()
        {
            if (inputActions.MelleAttack != null)
                inputActions.MelleAttack.performed -= OnMeleeAttackInput;

            if (inputActions.DistanceAttack != null)
                inputActions.Dash.performed -= OnDistanceAttackInput;
        }
        
    }
}