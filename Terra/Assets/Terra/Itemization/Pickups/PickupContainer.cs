using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Interfaces;
using Terra.Itemization.Abstracts;
using Terra.Managers;
using Terra.Player;
using UnityEngine;

namespace Terra.Itemization.Pickups
{

    /// <summary>
    /// Represents a container for a single Pickup item type
    /// </summary>
    public sealed class PickupContainer : Entity, IPickupable, IRequireCleanup
    {

        public bool CanBePickedUp => _isInitialized && _pickup.CanBePickedUp();

        [SerializeField, ReadOnly] private PickupBase _pickup;
        [SerializeField, ReadOnly] private bool _isInitialized;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _distanceToPlayerForMagnetism = 3f;
        [SerializeField] private float _onSpawnDelayBeforePickupable = 0.5f;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField, Range(0, 1f)] private float _addSpeedModifier = 0.05f;
        private float _originalSpeed;
        Tween _tween;
        private void Awake()
        {
            _tween = VFXcontroller.Model.transform
                .DOLocalMoveY(0.25f, 0.75f)
                .SetRelative()        
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
            _originalSpeed = _moveSpeed;
        }

        public void Initialize(PickupBase pickup)
        {
            _pickup = pickup;
            VFXcontroller.SetModelSprite(pickup.ItemIcon);
            VFXcontroller.SetModelMaterial(pickup.ItemMaterial);

            _ = DelayPickupActivation();
        }

        private async UniTaskVoid DelayPickupActivation()
        {
            await UniTask.WaitForSeconds(_onSpawnDelayBeforePickupable, cancellationToken: CancellationToken);
            _isInitialized = true;
        }
        private void OnTriggerStay(Collider collision)
        {
            if(_pickup == null) return;
            if (collision.CompareTag("Player") && CanBePickedUp)
            {
                PickUp();
            }
        }

        private void Update()
        {
            if(_pickup == null) return;
            
            if(!CanBePickedUp) return;
            
            if (GetDistanceToPlayer() < _distanceToPlayerForMagnetism)
            {
                MoveTowardsPlayer();
            }
            else
            {
                _moveSpeed = _originalSpeed;
            }
        }

        private void MoveTowardsPlayer()
        {
            Vector3 position = Vector3.MoveTowards(transform.position, PlayerManager.Instance.CurrentPosition, _moveSpeed * Time.deltaTime);
            position.y = transform.position.y;
            transform.position = position;
            _moveSpeed += _addSpeedModifier;
            _moveSpeed = Mathf.Clamp(_moveSpeed, _originalSpeed, _originalSpeed *3);
        }
        private float GetDistanceToPlayer()
        {
            return Vector3.Distance(PlayerManager.Instance.CurrentPosition, transform.position);
        }


        public void PickUp()
        {
            if(_pickup == null) return;
            if (!CanBePickedUp) return;
            if (_pickup.PickupSound && _audioSource)
            {
                AudioManager.Instance?.PlaySFXAtSource(_pickup.PickupSound, _audioSource);
            }
            _pickup.OnPickUp();
            Destroy(gameObject);
        }
        
        
        protected override void CleanUp()
        {
            _tween?.Kill();

            base.CleanUp();
        }

        public void PerformCleanup()
        {
            //TODO: Change to Pooling
            Destroy(gameObject);
        }
    }
}