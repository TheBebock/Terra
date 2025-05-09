using System;
using DG.Tweening;
using NaughtyAttributes;
using Terra.Player;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items.Definitions;
using UnityEngine;

namespace Terra.Itemization.Items
{

    /// <summary>
    /// Represents a container for a single Item type
    /// </summary>
    public class ItemContainer : InteractableBase
    {
        public override bool CanBeInteractedWith
        {
            get => isInitialized && PlayerInventoryManager.Instance.CanEquipItem(item);
            protected set { }
        }

        [SerializeField, ReadOnly] private bool isInitialized = false;

        [SerializeField, ReadOnly] private ItemBase item;
        
        [SerializeField] private SpriteRenderer itemRenderer;

        Tween tween;
        protected override void Awake()
        {
            base.Awake();
            tween = itemRenderer.transform
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
            isInitialized = true;
            this.item = item;
            if(item.ItemIcon)
                itemRenderer.sprite = item.ItemIcon;
  
        }
        

        public override void OnInteraction()
        {
            if (PlayerInventoryManager.Instance.TryToEquipItem(item))
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
            tween?.Kill();

            base.CleanUp();
        }
    }
}