
using Inventory.Abstracts;
using Player;
using UnityEngine;
public class ItemContainer : MonoBehaviour, IInteractable
{
    public bool CanBeInteractedWith => true;

    [SerializeField] private Item _item;

    //TODO: Delete
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact() 
    {
        if (PlayerInventoryManager.Instance.TryToEquipItem(_item))
        {
            OnInteraction();
        }
    }

    public void OnInteraction()
    {
        //TODO: Display VFX
    }


    public void ShowVisualisation()
    {
        
        if (CanBeInteractedWith)
        {
            if (PlayerInventoryManager.Instance.CanEquipItem(_item))
            {
                ShowAvailableVisualization();
                return;
            }
            ShowUnAvailableVisualization();
        }
    }


    public void ShowAvailableVisualization()
    {
        //TODO:Implement UI display
    }

    public void ShowUnAvailableVisualization()
    {
        //TODO:Implement UI display
    }

    public void StopVisualization()
    {
        //TODO:Stop UI display
    }
}
