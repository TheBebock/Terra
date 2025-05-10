using System;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Terra.Combat;
using Terra.InputManagement;
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
        
        private PlayerInventoryManager _playerInventory;

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

        private PlayerAttackDirection _currentplayerAttackDirection;
        public PlayerAttackDirection CurrentPlayerAttackDirection => _currentplayerAttackDirection;

        public enum PlayerAttackDirection
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3,
        }

        //NOTE: variable dummy was made to not override default constructor, as it was trying to construct it for serialization
        public PlayerAttackController(bool dummy)
        {
            if (InputManager.Instance)
            {
                inputActions = InputManager.Instance.PlayerControls;
            }
            else
            {
                Debug.LogError($"{this}: Input Manager not found.");
            }

            if (PlayerInventoryManager.Instance)
            {
                _playerInventory = PlayerInventoryManager.Instance;
            }
            else
            {
                Debug.LogError($"{this}: Player Inventory Manager not found.");
            }
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
            maxMeleeCD = _playerInventory.MeleeWeapon.Data.attackSpeed;
            if (currentMeleeCD == 0)
            {
                ChangeAttackDirection();
                isTryingPerformMeleeAttack = true;
                currentMeleeCD = maxMeleeCD;
                _playerInventory.StartCoroutine(DecreaseMeleeCooldown());
            }
        }

        private void OnDistanceAttackInput(InputAction.CallbackContext context)
        {
            //TODO: Ranged attack
            return;
            
            maxRangedCD = _playerInventory.RangedWeapon.Data.attackSpeed;
            if (currentRangedCD == 0)
            {
                ChangeAttackDirection();
                isTryingPerformDistanceAttack = true;
                currentRangedCD = maxRangedCD;
                _playerInventory.StartCoroutine(DecreaseRangedCooldown());
            }
        }

        private void ChangeAttackDirection()
        {
            if(isTryingPerformMeleeAttack) return;
            
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay( mousePosition );
            Plane plane = new Plane(Vector3.up, _playerInventory.transform.position);

            // Raycast get point where player clicked on screen while we use perspective camera
            if (!plane.Raycast(ray, out float enter)) return;
            
            
            Vector3 worldClickPosition = ray.GetPoint(enter);
            Vector3 direction = (worldClickPosition - _playerInventory.transform.position).normalized;

            if(Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                if(direction.x > 0) _currentplayerAttackDirection = PlayerAttackDirection.Right;
                else _currentplayerAttackDirection = PlayerAttackDirection.Left;
            }
            else
            {
                if(direction.z > 0) _currentplayerAttackDirection = PlayerAttackDirection.Up;
                else _currentplayerAttackDirection = PlayerAttackDirection.Down;
            }
            
            //TODO: Refactor later - move to attack state

            float range = _playerInventory.MeleeWeapon.Data.range;
            
            Vector3 attackPosition = _playerInventory.transform.position + direction * range;
            
            Debug.Log("Attack position" + attackPosition);
            List<IDamageable> targets = ComponentProvider.GetTargetsInSphere<IDamageable>(attackPosition, range, ComponentProvider.PlayerTargetsMask);

            CombatManager.Instance.PlayerPerformedAttack(targets, _playerInventory.MeleeWeapon.Data.damage);

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