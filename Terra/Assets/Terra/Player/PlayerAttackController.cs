using System;
using NaughtyAttributes;
using System.Collections;
using Terra.Enums;
using Terra.Interfaces;
using Terra.Itemization.Abstracts.Definitions;
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

        [SerializeField] private AudioClip _attackSFX;

        private AudioSource _audioSource;
        public static event Action<FacingDirection> OnMeleeAttackPerformed;
        
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
        

        //NOTE: variable dummy was made to not override default constructor, as it was trying to construct it for serialization
        public PlayerAttackController(AudioSource audioSource)
        {
            _audioSource = audioSource;
            
            if (PlayerInventoryManager.Instance)
            {
                _playerInventory = PlayerInventoryManager.Instance;
                _attackSFX = _playerInventory.MeleeWeapon.Data.attackSFX;
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
            InputManager.Instance.PlayerControls.DistanceAttack.performed += OnDistanceAttackInput;
            PlayerInventoryManager.Instance.OnMeleeWeaponChanged += OnMeleeWeaponChanged;
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
                _currentMeleeCd -=0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnMeleeAttackInput(InputAction.CallbackContext context)
        {
            Debug.Log($"Input test ${context.ReadValue<float>()} ");
            if (_currentMeleeCd <= 0 && !_isTryingPerformMeleeAttack)
            {
                AudioManager.Instance.PlaySFXAtSource(_attackSFX, _audioSource);
                ChangeAttackDirection();
                _isTryingPerformMeleeAttack = true;
                _currentMeleeCd = _maxMeleeCd;
                OnMeleeAttackPerformed?.Invoke(_currentPlayerAttackDirection);
                _playerInventory.StartCoroutine(DecreaseMeleeCooldown());
            }
        }

        private void OnDistanceAttackInput(InputAction.CallbackContext context)
        {
            //TODO: Ranged attack
            return;
            
            if (_currentRangedCd <= 0 && !_isTryingPerformDistanceAttack)
            {
                ChangeAttackDirection();
                _isTryingPerformDistanceAttack = true;
                _currentRangedCd = _maxRangedCd;
                _playerInventory.StartCoroutine(DecreaseRangedCooldown());
            }
        }

        private void OnMeleeWeaponChanged(MeleeWeapon meleeWeapon)
        {
            _attackSFX = meleeWeapon.Data.attackSFX;
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
                InputManager.Instance.PlayerControls.Dash.performed -= OnDistanceAttackInput;
            }

            if (PlayerInventoryManager.Instance)
            {
                PlayerInventoryManager.Instance.OnMeleeWeaponChanged -= OnMeleeWeaponChanged;
            }
        }
    }
}