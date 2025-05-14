using System;
using NaughtyAttributes;
using System.Collections;
using Terra.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terra.Player
{

    /// <summary>
    /// Handles player attacks
    /// </summary>
    [Serializable]
    public class PlayerAttackController : IAttachListeners
    {
        [Foldout("Debug"), ReadOnly] [SerializeField]
        
        private PlayerInventoryManager _playerInventory;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private bool _isTryingPerformMeleeAttack;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private bool _isTryingPerformDistanceAttack;


        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float _currentMeleeCd;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float _currentRangedCd;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float _maxMeleeCd;

        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float _maxRangedCd;

        public bool IsTryingPerformMeleeAttack
        {
            get => _isTryingPerformMeleeAttack;
            set => _isTryingPerformMeleeAttack = value;

        }

        public bool IsTryingPerformDistanceAttack
        {
            get => _isTryingPerformDistanceAttack;
            set => _isTryingPerformDistanceAttack = value;
        }

        public float CurrentMeleeCooldown => _currentMeleeCd;
        public float CurrentRangedCooldown => _currentRangedCd;

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
            InputManager.Instance.PlayerControls.MeleeAttack.performed += OnMeleeAttackInput;
            InputManager.Instance.PlayerControls.DistanceAttack.performed += OnDistanceAttackInput;
        }

        private IEnumerator DecreaseMeleeCooldown()
        {
            while (CurrentMeleeCooldown > 0)
            {
                _currentMeleeCd--;
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator DecreaseRangedCooldown()
        {
            while (CurrentRangedCooldown > 0)
            {
                _currentRangedCd--;
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnMeleeAttackInput(InputAction.CallbackContext context)
        {
            _maxMeleeCd = _playerInventory.MeleeWeapon.Data.attackSpeed;
            if (_currentMeleeCd == 0)
            {
                ChangeAttackDirection();
                _isTryingPerformMeleeAttack = true;
                _currentMeleeCd = _maxMeleeCd;
                _playerInventory.StartCoroutine(DecreaseMeleeCooldown());
            }
        }

        private void OnDistanceAttackInput(InputAction.CallbackContext context)
        {
            //TODO: Ranged attack
            return;
            
            _maxRangedCd = _playerInventory.RangedWeapon.Data.attackSpeed;
            if (_currentRangedCd == 0)
            {
                ChangeAttackDirection();
                _isTryingPerformDistanceAttack = true;
                _currentRangedCd = _maxRangedCd;
                _playerInventory.StartCoroutine(DecreaseRangedCooldown());
            }
        }

        private void ChangeAttackDirection()
        {
            if(_isTryingPerformMeleeAttack) return;
            
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            //TODO: Change to camera manager
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
            Quaternion targetRotation = Quaternion.LookRotation(attackPosition - _playerInventory.transform.position);
            Debug.Log($"Attack position: {attackPosition}\nTarget rotation: {targetRotation}");

            _playerInventory.MeleeWeapon.PerformAttack(attackPosition, targetRotation);

        }
        
        public void DetachListeners()
        {
            if (InputManager.Instance)
            {
                InputManager.Instance.PlayerControls.MeleeAttack.performed -= OnMeleeAttackInput;
                InputManager.Instance.PlayerControls.Dash.performed -= OnDistanceAttackInput;
            }
        }
    }
}