
using Inventory.Abstracts;
using Player;
using UnityEngine;
public class ItemContainer : InteractableBase
{
    override public bool CanBeInteractedWith => PlayerInventoryManager.Instance.CanEquipItem(item);

    [SerializeField] private Item item;
    //This works like shit, use InputManager to check for input
    //TODO: Delete
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Interact();
            ShowVisualisation();
        }
    }

    override public void Interact() 
    {
        if(!CanBeInteractedWith) return;
        if (PlayerInventoryManager.Instance.TryToEquipItem(item))
        {
            OnInteraction();
        }
    }

    override public void OnInteraction()
    {
        //TODO: Display VFX
    }

    protected override void ShowAvailableVisualization()
    {
        base.ShowAvailableVisualization();
        //TODO: Display VFX
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
