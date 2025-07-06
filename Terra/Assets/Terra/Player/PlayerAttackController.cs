using System;
using NaughtyAttributes;
using Terra.Combat.Projectiles;
using Terra.EffectsSystem;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Abstract.Definitions;
using Terra.Enums;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.InputSystem;
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
        //NOTE: Due to ground not rotating towards camera compared to all other entities, there needs to be an offset
        private Vector3 _raycastOffset = new(0,0, -1.85f);
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
        
        
        [Foldout("Debug"), ReadOnly] [SerializeField] private AudioClip _meleeAttackSFX;
        [Foldout("Debug"), ReadOnly] [SerializeField] private AudioClip _rangedAttackSFX;
        [Foldout("Debug"), ReadOnly] [SerializeField] private AudioSource _audioSource;
        [Foldout("Debug"), ReadOnly] [SerializeField] private bool _isDashing = false;

        public EffectsContainer MeleeEffectContainer => _meleeEffectContainer;
        public EffectsContainer RangeEffectContainer => _rangeEffectContainer;
        public bool IsTryingPerformMeleeAttack => _isTryingPerformMeleeAttack;
        
        public bool IsTryingPerformDistanceAttack => _isTryingPerformDistanceAttack;

        private FacingDirection _currentPlayerAttackDirection;
        public FacingDirection CurrentPlayerAttackDirection => _currentPlayerAttackDirection;
        public PlayerInventoryManager PlayerInventory => PlayerInventoryManager.Instance;
        private OnPlayerMeleeAttackPerformedEvent _meleeEvent;

        private OnEffectAddedToPlayer _onEffectAddedToPlayer;

        private float _lastMeleeAttackTime = -Mathf.Infinity;
        private float _lastRangeAttackTime = -Mathf.Infinity;
        
        private Vector3 _rangeShootDirection;
        public PlayerAttackController(AudioSource audioSource, Transform firePoint)
        {
            _audioSource = audioSource;
            _firePoint = firePoint;
            _meleeEvent = new();
            _onEffectAddedToPlayer = new();
            if (PlayerInventoryManager.Instance)
            {
                _meleeAttackSFX = PlayerInventory.MeleeWeapon.Data.attackSFX;
                _rangedAttackSFX = PlayerInventory.RangedWeapon.Data.attackSFX;
            }
            else
            {
                Debug.LogError($"{this}: Player Inventory Manager not found.");
            }
        }
        public void AttachListeners()
        {
            EventsAPI.Register<OnPlayerDashStartedEvent>(OnDashStarted);
            EventsAPI.Register<OnPlayerDashEndedEvent>(OnDashEnded);
            InputsManager.Instance.PlayerControls.MeleeAttack.started += OnMeleeAttackInput;
            InputsManager.Instance.PlayerControls.RangeAttack.started += OnRangeAttackInput;
            PlayerInventory.OnMeleeWeaponChanged += OnMeleeWeaponChanged;
            PlayerInventory.OnRangedWeaponChanged += OnRangedWeaponChanged;
        }
        

        private void OnMeleeAttackInput(InputAction.CallbackContext context)
        {
            
            
            if(TimeManager.Instance.IsGamePaused) return;
            
            float currentTime = Time.time;
            if (currentTime - _lastMeleeAttackTime < 0.2f)
            {
                return;
            }
            
            if (!_isTryingPerformMeleeAttack && !_isTryingPerformDistanceAttack)
            {
                _isTryingPerformMeleeAttack = true;
                _lastMeleeAttackTime = currentTime;
                
                Vector3 direction = MouseRaycastManager.Instance.
                    GetDirectionTowardsMousePosition(PlayerInventory.transform.position, _raycastOffset);
                ChangeAttackDirection(direction);

                PlayerManager.Instance.PlayerMovement.PushPlayerInDirection(direction, PlayerManager.Instance.PushPlayerForce);
                
                AudioManager.Instance.PlaySFXAtSource(_meleeAttackSFX, _audioSource, true);
                _meleeEvent.facingDirection = _currentPlayerAttackDirection;
                EventsAPI.Invoke(ref _meleeEvent);
            }
        }

        private void OnRangeAttackInput(InputAction.CallbackContext context)
        {
            if(TimeManager.Instance.IsGamePaused) return;

            float currentTime = Time.time;
            if (currentTime - _lastRangeAttackTime < 0.2f)
            {
                return;
            }
            
            if (_firePoint == null)
            {
                Debug.LogError($"{this} tried to performed range attack without assigend fire point. Hash: {this.GetHashCode()}");
                return;
            }
            if(PlayerInventoryManager.Instance.CurrentAmmo <= 0) return;
            
            if (!_isTryingPerformDistanceAttack && !_isTryingPerformMeleeAttack)
            {
                _isTryingPerformDistanceAttack = true;
                _lastRangeAttackTime = currentTime;
                
                Vector3 direction = MouseRaycastManager.Instance.
                    GetDirectionTowardsMousePosition(PlayerInventory.transform.position, _raycastOffset);
                ChangeAttackDirection(direction);
                
                _rangeShootDirection =
                    MouseRaycastManager.Instance.GetDirectionTowardsMousePosition(_firePoint.position, _raycastOffset);
            }
        }

        public void PerformRangeAttack()
        {
            if(!_isTryingPerformDistanceAttack) return;
            
            AudioManager.Instance.PlaySFXAtSource(_rangedAttackSFX, _audioSource, true);
            EventsAPI.Invoke<OnPlayerRangeAttackPerformedEvent>();
                
            ProjectileFactory.CreateProjectile(
                PlayerInventory.RangedWeapon.Data.bulletData,
                _firePoint.position, 
                _rangeShootDirection,
                PlayerManager.Instance.PlayerEntity);
        }
        private void OnMeleeWeaponChanged(MeleeWeapon meleeWeapon)
        {
            _meleeAttackSFX = meleeWeapon.Data.attackSFX;
        }

        private void OnRangedWeaponChanged(RangedWeapon rangedWeapon)
        {
            _rangedAttackSFX = rangedWeapon.Data.attackSFX;
        }
        
        private void ChangeAttackDirection(Vector3 direction)
        {
            if(_isTryingPerformMeleeAttack) return;
            
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
            EffectsContainer effectsContainer;
            
            _onEffectAddedToPlayer.effectSprite = action.effectIcon;
            
            EventsAPI.Invoke(ref _onEffectAddedToPlayer);
            
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
            EffectsContainer effectsContainer;
            _onEffectAddedToPlayer.effectSprite = status.effectIcon;

            EventsAPI.Invoke(ref _onEffectAddedToPlayer);

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

        private void OnDashStarted(ref OnPlayerDashStartedEvent dashEvent)
        {
            _isDashing = true;
        }

        private void OnDashEnded(ref OnPlayerDashEndedEvent dashEvent)
        {
            _isDashing = false;
        }
        public void DetachListeners()
        {
            EventsAPI.Unregister<OnPlayerDashStartedEvent>(OnDashStarted);
            EventsAPI.Unregister<OnPlayerDashEndedEvent>(OnDashEnded);
            
            if (InputsManager.Instance)
            {
                InputsManager.Instance.PlayerControls.MeleeAttack.started -= OnMeleeAttackInput;
                InputsManager.Instance.PlayerControls.RangeAttack.started -= OnRangeAttackInput;
            }
            else
            {
                Debug.LogError($"Player Attack controller couldnt detach inputs listeners, as there is no inputs manager");
            }

            if (PlayerInventoryManager.Instance)
            {
                PlayerInventoryManager.Instance.OnMeleeWeaponChanged -= OnMeleeWeaponChanged;
                PlayerInventoryManager.Instance.OnRangedWeaponChanged -= OnRangedWeaponChanged;
            }
        }
    }
}