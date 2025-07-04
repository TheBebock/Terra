using DG.Tweening;
using NaughtyAttributes;
using Terra.Interactions;
using Terra.Interfaces;
using Terra.Player;
using Terra.Itemization.Abstracts;
using UnityEngine;

namespace Terra.Itemization.Items
{

    /// <summary>
    /// Represents a container for a single Item type
    /// </summary>
    public sealed class ItemContainer : InteractableBase, IRequireCleanup
    {
        public override bool CanBeInteractedWith => _isInitialized && PlayerInventoryManager.Instance.CanEquipItem(_item);

        [SerializeField, ReadOnly] private bool _isInitialized;

        [SerializeField, ReadOnly] private ItemBase _item;
        
        [SerializeField] private SpriteRenderer _itemRenderer;

        Tween _tween;
        private void Awake()
        {
            _tween = _itemRenderer.transform
                .DOLocalMoveY(0.25f, 0.75f)
                .SetRelative()        
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine); 
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