using System;
using NaughtyAttributes;
using System.Collections;
using Terra.Combat.Projectiles;
using Terra.Enums;
using Terra.Interfaces;
using Terra.Itemization.Items;
using Terra.Managers;
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
        private Transform _firePoint;
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

        [SerializeField] private AudioClip _meleeAttackSFX;
        [SerializeField] private AudioClip _rangedAttackSFX;

        private AudioSource _audioSource;
        public static event Action<FacingDirection> OnMeleeAttackPerformed;
        public static event Action OnRangeAttackPerformed;
        
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

        private FacingDirection _currentPlayerAttackDirection;
        public FacingDirection CurrentPlayerAttackDirection => _currentPlayerAttackDirection;
        

        //NOTE: variable made to not override default constructor, as it was trying to construct it for serialization
        public PlayerAttackController(AudioSource audioSource, Transform firePoint)
        {
            _audioSource = audioSource;
            _firePoint = firePoint;
            if (PlayerInventoryManager.Instance)
            {
                _playerInventory = PlayerInventoryManager.Instance;
                _meleeAttackSFX = _playerInventory.MeleeWeapon.Data.attackSFX;
                _rangedAttackSFX = _playerInventory.RangedWeapon.Data.attackSFX;
                _maxMeleeCd = _playerInventory.MeleeWeapon.Data.attackCooldown;
                _maxRangedCd = _playerInventory.RangedWeapon.Data.attackCooldown;
            }
            else
            {
                Debug.LogError($"{this}: Player Inventory Manager not found.");
            }
        }

        public void AttachListeners()
        {
            InputManager.Instance.PlayerControls.MeleeAttack.performed += OnMeleeAttackInput;
            InputManager.Instance.PlayerControls.RangeAttack.performed += OnRangeAttackInput;
            PlayerInventoryManager.Instance.OnMeleeWeaponChanged += OnMeleeWeaponChanged;
            PlayerInventoryManager.Instance.OnRangedWeaponChanged += OnRangedWeaponChanged;
        }

        private IEnumerator DecreaseMeleeCooldown()
        {
            while (CurrentMeleeCooldown > 0)
            {
                _currentMeleeCd -=0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator DecreaseRangedCooldown()
        {
            while (CurrentRangedCooldown > 0)
            {
                _currentRangedCd -=0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnMeleeAttackInput(InputAction.CallbackContext context)
        {
            if (_currentMeleeCd <= 0 && !_isTryingPerformMeleeAttack)
            {
                AudioManager.Instance.PlaySFXAtSource(_meleeAttackSFX, _audioSource);
                ChangeAttackDirection();
                _isTryingPerformMeleeAttack = true;
                _currentMeleeCd = _maxMeleeCd;
                OnMeleeAttackPerformed?.Invoke(_currentPlayerAttackDirection);
                _playerInventory.StartCoroutine(DecreaseMeleeCooldown());
            }
        }

        private void OnRangeAttackInput(InputAction.CallbackContext context)
        {
            if (_currentRangedCd <= 0 && !_isTryingPerformDistanceAttack)
            {
                AudioManager.Instance.PlaySFXAtSource(_rangedAttackSFX, _audioSource);
                ChangeAttackDirection();
                _isTryingPerformDistanceAttack = true;
                _currentRangedCd = _maxRangedCd;
                OnRangeAttackPerformed?.Invoke();
                ProjectileFactory.CreateProjectile(
                    _playerInventory.RangedWeapon.Data.bulletData,
                    _firePoint.position, 
                    MouseRaycastManager.Instance.GetDirectionTowardsMousePosition(_firePoint.position),
                    PlayerManager.Instance.PlayerEntity);
                _playerInventory.StartCoroutine(DecreaseRangedCooldown());
            }
        }

        private void OnMeleeWeaponChanged(MeleeWeapon meleeWeapon)
        {
            _meleeAttackSFX = meleeWeapon.Data.attackSFX;
        }

        private void OnRangedWeaponChanged(RangedWeapon rangedWeapon)
        {
            _rangedAttackSFX = rangedWeapon.Data.attackSFX;
        }
        
        private void ChangeAttackDirection()
        {
            if(_isTryingPerformMeleeAttack) return;
            
            Vector3 direction = MouseRaycastManager.Instance.GetDirectionTowardsMousePosition(_playerInventory.transform.position);

            if(Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                if(direction.x > 0) _currentPlayerAttackDirection = FacingDirection.Right;
                else _currentPlayerAttackDirection = FacingDirection.Left;
            }
            else
            {
                if(direction.z > 0) _currentPlayerAttackDirection = FacingDirection.Up;
                else _currentPlayerAttackDirection = FacingDirection.Down;
            }
        }
        
        public void DetachListeners()
        {
            if (InputManager.Instance)
            {
                InputManager.Instance.PlayerControls.MeleeAttack.performed -= OnMeleeAttackInput;
                InputManager.Instance.PlayerControls.Dash.performed -= OnRangeAttackInput;
            }

            if (PlayerInventoryManager.Instance)
            {
                PlayerInventoryManager.Instance.OnMeleeWeaponChanged -= OnMeleeWeaponChanged;
                PlayerInventoryManager.Instance.OnRangedWeaponChanged -= OnRangedWeaponChanged;
            }
        }
    }
}