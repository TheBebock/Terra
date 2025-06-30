using System;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Terra.Combat.Projectiles;
using Terra.EffectsSystem;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Abstract.Definitions;
using Terra.Enums;
using Terra.Extensions;
using Terra.InputSystem;
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
        private Transform _firePoint;
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private bool _isTryingPerformMeleeAttack;
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private bool _isTryingPerformDistanceAttack;
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float _maxMeleeCd;
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private float _maxRangedCd;
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private EffectsContainer _meleeEffectContainer = new ();        
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private EffectsContainer _rangeEffectContainer = new ();
        
        [SerializeField] private AudioClip _meleeAttackSFX;
        [SerializeField] private AudioClip _rangedAttackSFX;

        private AudioSource _audioSource;
        public static event Action<FacingDirection> OnMeleeAttackPerformed;
        public static event Action OnRangeAttackPerformed;
        
        public EffectsContainer MeleeEffectContainer => _meleeEffectContainer;
        public EffectsContainer RangeEffectContainer => _rangeEffectContainer;
        public bool IsTryingPerformMeleeAttack => _isTryingPerformMeleeAttack;
        
        public bool IsTryingPerformDistanceAttack => _isTryingPerformDistanceAttack;

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
            }
            else
            {
                Debug.LogError($"{this}: Player Inventory Manager not found.");
            }
        }

        public void AttachListeners()
        {
            InputManager.Instance.PlayerControls.MeleeAttack.started += OnMeleeAttackInput;
            InputManager.Instance.PlayerControls.RangeAttack.started += OnRangeAttackInput;
            PlayerInventoryManager.Instance.OnMeleeWeaponChanged += OnMeleeWeaponChanged;
            PlayerInventoryManager.Instance.OnRangedWeaponChanged += OnRangedWeaponChanged;
        }
        

        private void OnMeleeAttackInput(InputAction.CallbackContext context)
        {
            if (!_isTryingPerformMeleeAttack)
            {
                ChangeAttackDirection();

                _isTryingPerformMeleeAttack = true;
                AudioManager.Instance.PlaySFXAtSource(_meleeAttackSFX, _audioSource);
                OnMeleeAttackPerformed?.Invoke(_currentPlayerAttackDirection);
            }
        }

        private void OnRangeAttackInput(InputAction.CallbackContext context)
        {
            if (!_isTryingPerformDistanceAttack)
            {
                ChangeAttackDirection();

                
                _isTryingPerformDistanceAttack = true;
                AudioManager.Instance.PlaySFXAtSource(_rangedAttackSFX, _audioSource);
                OnRangeAttackPerformed?.Invoke();
                ProjectileFactory.CreateProjectile(
                    _playerInventory.RangedWeapon.Data.bulletData,
                    _firePoint.position, 
                    MouseRaycastManager.Instance.GetDirectionTowardsMousePosition(_firePoint.position),
                    PlayerManager.Instance.PlayerEntity);
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

        public void AddNewAttackActionEffect(ActionEffectData action)
        {
            EffectsContainer effectsContainer = null;
            switch (action.containerType)
            {
                case ContainerType.MeleeWeapon: 
                    effectsContainer = _meleeEffectContainer; 
                    break;
                case ContainerType.RangedWeapon:
                    effectsContainer = _rangeEffectContainer;
                    break;
                
                case ContainerType.AllWeapons:
                    _meleeEffectContainer.AddNewActionEffect(action, action.incompatibleEffects);
                    _rangeEffectContainer.AddNewActionEffect(action, action.incompatibleEffects);
                    return;
                
                default:
                    Debug.LogError($"{this}: {action} has invalid container type. No effects can be added.");
                    return;
            }
            
            effectsContainer.AddNewActionEffect(action, action.incompatibleEffects);
        }
        
        public void AddNewAttackStatusEffect(StatusEffectData status)
        {
            EffectsContainer effectsContainer = null;
            switch (status.containerType)
            {
                case ContainerType.MeleeWeapon: 
                    effectsContainer = _meleeEffectContainer; 
                    break;
                case ContainerType.RangedWeapon:
                    effectsContainer = _rangeEffectContainer;
                    break;
                case ContainerType.AllWeapons:
                    _meleeEffectContainer.AddNewStatusEffect(status, status.incompatibleEffects);
                    _rangeEffectContainer.AddNewStatusEffect(status, status.incompatibleEffects);
                    return;
                
                default:
                    Debug.LogError($"{this}: {status} has invalid container type. No effects can be added.");
                    return;
            }
            
            effectsContainer.AddNewStatusEffect(status, status.incompatibleEffects);
        }
        
        public void OnMeleeAnimationEnd() => _isTryingPerformMeleeAttack = false;
        public void OnRangeAnimationEnd() => _isTryingPerformDistanceAttack = false;
        
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