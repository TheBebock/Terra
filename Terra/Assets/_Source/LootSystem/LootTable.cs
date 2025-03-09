using System.Collections;
using System.Collections.Generic;
using Inventory.Abstracts;
using Inventory.Items;
using Inventory.Items.Definitions;
using Inventory.Pickups;
using Inventory.Pickups.Definitions;
using NaughtyAttributes;
using UnityEngine;

namespace Terra.LootSystem
{
    [CreateAssetMenu(fileName = "LootTable", menuName = "TheBebocks/LootTable")]
    public class LootTable : ScriptableObject
    {
        [Foldout("References")][SerializeField] private ItemContainer P_ItemContainer;
        [Foldout("References")][SerializeField] private PickupContainer P_PickupContainer;
        
        // TODO: Make a list for each category, change All... from variable to Method() that links all the lists
        [SerializeField] private List<Item> AllItems = new ();
        [SerializeField] private List<Pickup> AllPickups = new ();
        [SerializeField] private List<PassiveItem> PassiveItems = new ();
        [SerializeField] private List<RangedWeapon> RangedWeapons = new ();
        public List<Item> GetRandomItemsFromEachCategory()
        {
            List<Item> selectedItems = new List<Item>();
            
            var rangedWeapons = GetRandomItemFromList(AllItems.FindAll(i => i.itemType == ItemType.Ranged));
            if (rangedWeapons != null) selectedItems.Add(rangedWeapons);

            var meleeWeapons = GetRandomItemFromList(AllItems.FindAll(i => i.itemType== ItemType.Melee));
            if (meleeWeapons != null) selectedItems.Add(meleeWeapons);

            var passiveItems = GetRandomItemFromList(AllItems.FindAll(i => i.itemType == ItemType.Passive));
            if (passiveItems != null) selectedItems.Add(passiveItems);

            var activeItems = GetRandomItemFromList(AllItems.FindAll(i => i.itemType== ItemType.Active));
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

            Pickup healthPickups = GetRandomPickupFromList(AllPickups.FindAll(p => p.PickupType == PickupType.Health));
            if (healthPickups != null) selectedPickups.Add(healthPickups);

            Pickup ammoPickups = GetRandomPickupFromList(AllPickups.FindAll(p => p.PickupType == PickupType.Ammo));
            if (ammoPickups != null) selectedPickups.Add(ammoPickups);
            
            Pickup crystalPickups = GetRandomPickupFromList(AllPickups.FindAll(p => p.PickupType == PickupType.Crystal));
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
    