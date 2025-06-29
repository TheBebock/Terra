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
    public sealed class PickupContainer : InGameMonobehaviour, IPickupable
    {
        public bool CanBePickedUp { get; private set; }

        [SerializeField, ReadOnly] private PickupBase _pickup;
        [SerializeField] private SpriteRenderer _pickupRenderer;
        [SerializeField] private float _distanceToPlayerForMagnetism = 3f;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField, Range(0, 1f)] private float _addSpeedModifier = 0.05f;
        private float _originalSpeed;
        Tween _tween;
        private void Awake()
        {
            _tween = _pickupRenderer.transform
                .DOLocalMoveY(0.25f, 0.75f)
                .SetRelative()        
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
            _originalSpeed = _moveSpeed;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag("Player") && CanBePickedUp && _pickup != null)
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
            _pickupRenderer.sprite = pickup.ItemIcon;
            _pickupRenderer.material = pickup.ItemMaterial;
            CanBePickedUp = true;
        }

        public void PickUp()
        {
            if (!CanBePickedUp) return;
            _pickup.OnPickUp();
            Destroy(gameObject);
        }

        public void SetAvailability(bool isAvailable)
        {
            CanBePickedUp = isAvailable;
        }
        
        protected override void CleanUp()
        {
            _tween?.Kill();

            base.CleanUp();
        }
    }
}