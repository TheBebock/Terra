
using System.Collections.Generic;
using Inventory.Abstracts;
using NaughtyAttributes;
using Player;
using UnityEngine;
public class ItemContainer : InteractableBase
{
    override public bool CanBeInteractedWith => PlayerInventoryManager.Instance.CanEquipItem(item);

    [SerializeField, ReadOnly] private Item item;
    [SerializeField, ReadOnly] private List<Item> rangedWeapons = new List<Item>();
    [SerializeField, ReadOnly] private List<Item> meleeWeapons = new List<Item>();
    [SerializeField, ReadOnly] private List<Item> activeItems = new List<Item>();
    [SerializeField, ReadOnly] private List<Item> passiveItems = new List<Item>();
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
    
    public List<Item> GetAllItems()
    {
        List<Item> allItems = new List<Item>();

        allItems.AddRange(rangedWeapons);
        allItems.AddRange(meleeWeapons);
        allItems.AddRange(passiveItems);
        allItems.AddRange(activeItems);

        return allItems;
    }
}
