using System.Collections;
using System.Collections.Generic;
using Inventory.Abstracts;
using Inventory.Items.Definitions;
using Inventory.Pickups;
using Inventory.Pickups.Definitions;
using UnityEngine;

namespace LootSystem
{
    public class LootTable : MonoBehaviour
    {
        public ItemContainer itemContainer;
        public PickupContainer pickupContainer;
        
        public List<Item> GetRandomItemsFromEachCategory()
        {
            List<Item> selectedItems = new List<Item>();
            
            var rangedWeapons = GetRandomItemFromList(itemContainer.GetAllItems().FindAll(i => i.itemType == ItemType.Ranged));
            if (rangedWeapons != null) selectedItems.Add(rangedWeapons);

            var meleeWeapons = GetRandomItemFromList(itemContainer.GetAllItems().FindAll(i => i.itemType== ItemType.Melee));
            if (meleeWeapons != null) selectedItems.Add(meleeWeapons);

            var passiveItems = GetRandomItemFromList(itemContainer.GetAllItems().FindAll(i => i.itemType == ItemType.Passive));
            if (passiveItems != null) selectedItems.Add(passiveItems);

            var activeItems = GetRandomItemFromList(itemContainer.GetAllItems().FindAll(i => i.itemType== ItemType.Active));
            if (activeItems!= null) selectedItems.Add(activeItems);

            return selectedItems;
        }
        private Item GetRandomItemFromList(List<Item> items)
        {
            if (items == null || items.Count == 0) return null;

            int index = Random.Range(0, items.Count);
            return items[index];
        }
        
        public List<Pickup> GetRandomPickupsFromEachCategory()
        {
            List<Pickup> selectedPickups = new List<Pickup>();

            var healthPickups = GetRandomPickupFromList(pickupContainer.GetAllPickups().FindAll(p => p.pickupType == PickupType.Health));
            if (healthPickups != null) selectedPickups.Add(healthPickups);

            var ammoPickups = GetRandomPickupFromList(pickupContainer.GetAllPickups().FindAll(p => p.pickupType == PickupType.Ammo));
            if (ammoPickups != null) selectedPickups.Add(ammoPickups);

            var crystalPickups = GetRandomPickupFromList(pickupContainer.GetAllPickups().FindAll(p => p.pickupType == PickupType.Crystal));
            if (crystalPickups != null) selectedPickups.Add(crystalPickups);

            return selectedPickups;
        }
        private Pickup GetRandomPickupFromList(List<Pickup> pickups)
        {
            if (pickups == null || pickups.Count == 0) return null;

            int index = Random.Range(0, pickups.Count);
            return pickups[index];
        }
    }
}
    