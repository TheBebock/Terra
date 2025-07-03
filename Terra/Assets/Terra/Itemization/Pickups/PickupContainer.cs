using System;
using DG.Tweening;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.Interfaces;
using Terra.Itemization.Abstracts;
using Terra.Player;
using UnityEngine;

namespace Terra.Itemization.Pickups
{

    /// <summary>
    /// Represents a container for a single Pickup item type
    /// </summary>
    public sealed class PickupContainer : Entity, IPickupable, IRequireCleanup
    {

        public bool CanBePickedUp => _isInitialized && _pickup != null && _pickup.CanBePickedUp();

        [SerializeField, ReadOnly] private PickupBase _pickup;
        [SerializeField, ReadOnly] private bool _isInitialized;

        [SerializeField] private float _distanceToPlayerForMagnetism = 3f;
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

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag("Player") && CanBePickedUp)
            {
                PickUp();
            }
        }

        private void Update()
        {
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
        }
        private float GetDistanceToPlayer()
        {
            return Vector3.Distance(PlayerManager.Instance.CurrentPosition, transform.position);
        }
        public void Initialize(PickupBase pickup)
        {
            _pickup = pickup;
            _isInitialized = true;
            VFXcontroller.SetModelSprite(pickup.ItemIcon);
            VFXcontroller.SetModelMaterial(pickup.ItemMaterial);
        }

        public void PickUp()
        {
            if (!CanBePickedUp) return;
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