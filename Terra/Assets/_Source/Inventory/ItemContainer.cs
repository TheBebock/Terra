using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemContainer : MonoBehaviour, IInteractable
{
    [SerializeField] private Item _item;
    
    public void Interact()
    {
        OnInteraction();
    }

    private void OnInteraction()
    {
        _item.Equip(_item);
    }
    
}
