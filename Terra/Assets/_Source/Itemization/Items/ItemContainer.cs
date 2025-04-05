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
        public override bool CanBeInteractedWith => isInitialized && PlayerInventoryManager.Instance.CanEquipItem(item);

        [SerializeField, ReadOnly] private bool isInitialized = false;

        [SerializeField, ReadOnly] private ItemBase item;
        
        [SerializeField] private SpriteRenderer itemRenderer;

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
            itemRenderer.sprite = item.ItemIcon;
  
        }

        public override void Interact()
        {
            if (!CanBeInteractedWith) return;
            if (PlayerInventoryManager.Instance.TryToEquipItem(item))
            {
                OnInteraction();
            }
        }

        public override void OnInteraction()
        {
            //TODO: Display VFX
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

    }
}