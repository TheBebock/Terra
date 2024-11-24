using Inventory.Items;
using Player;
using UnityEngine;
public class ItemContainer : MonoBehaviour, IInteractable
{
    [SerializeField] private Item _item;
    public void Interact()
    {
        OnInteraction();
    }

    private void OnInteraction()
    {
        if (_item is IEquipable<Item> equipable)
        {
            PlayerInventory.Instance.EquipItem(_item);
            Debug.Log($"Player equipped: {_item.data.itemName}");
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError($"Item is not equipable");
        }
    }
    
}
