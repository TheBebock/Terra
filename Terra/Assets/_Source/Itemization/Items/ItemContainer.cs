using System;
using DG.Tweening;
using NaughtyAttributes;
using Terra.Player;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items.Definitions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Itemization.Items
{

    /// <summary>
    /// Represents a container for a single Item type
    /// </summary>
    public class ItemContainer : InteractableBase
    {
        public override bool CanBeInteractedWith
        {
            get => _isInitialized && PlayerInventoryManager.Instance.CanEquipItem(_item);
            protected set { }
        }

        [SerializeField, ReadOnly] private bool _isInitialized = false;

        [SerializeField, ReadOnly] private ItemBase _item;
        
        [FormerlySerializedAs("itemRenderer")] [SerializeField] private SpriteRenderer _itemRenderer;

        Tween _tween;
        private void Awake()
        {
            _tween = _itemRenderer.transform
                .DOLocalMoveY(0.25f, 0.75f)
                .SetRelative()        
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine); 
        }

        //TODO: Delete
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }

        public void Initialize(ItemBase item)
        {
            _isInitialized = true;
            this._item = item;
            if(item.ItemIcon)
                _itemRenderer.sprite = item.ItemIcon;
  
        }
        

        public override void OnInteraction()
        {
            if (PlayerInventoryManager.Instance.TryToEquipItem(_item))
            {
                Destroy(gameObject);
            }
        }

        protected override void ShowAvailableVisualization()
        {
            base.ShowAvailableVisualization();
        }

        protected override void ShowUnAvailableVisualization()
        {
            base.ShowUnAvailableVisualization();
            //TODO: Display VFX
        }

        public override void StopVisualization()
        {
            base.StopVisualization();
            //NOTE: Maybe some additional logic
        }

        protected override void CleanUp()
        {
            _tween?.Kill();

            base.CleanUp();
        }
    }
}