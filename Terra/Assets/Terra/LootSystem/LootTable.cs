using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Terra.Extensions;
using Terra.Itemization;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Itemization.Items;
using Terra.Itemization.Pickups;
using UnityEngine;

namespace Terra.LootSystem
{
    /// <summary>
    /// Contains all possible items and PickupBase that can be dropped in the game.
    /// </summary>
    [Serializable]
    public class LootTable
    {
        [SerializeField, ReadOnly]
        private bool isInitialized = false;
        
        [SerializeField] private List<ActiveItem> activeItems = new (); 
        [SerializeField] private List<PassiveItem> passiveItems = new ();
        [SerializeField] private List<MeleeWeapon> meleeWeapons = new ();
        [SerializeField] private List<RangedWeapon> rangedWeapons = new ();
        
        [SerializeField] private List<HealthPickup> healthPickups = new ();
        [SerializeField] private List<AmmoPickup> ammoPickups = new ();
        [SerializeField] private List<CrystalPickup> crystalPickups = new ();

        public List<Item<ItemData>> GetAllItems()
        {
            List<Item<ItemData>> allItems = new List<Item<ItemData>>();
            allItems.AddRange(activeItems.Cast<Item<ItemData>>());
            allItems.AddRange(passiveItems.Cast<Item<ItemData>>());
            allItems.AddRange(meleeWeapons.Cast<Item<ItemData>>());
            allItems.AddRange(rangedWeapons.Cast<Item<ItemData>>());
            return allItems;
        }
        
        public List<PickupBase> GetAllPickups()
        {
            List<PickupBase> allPickups = new();
            allPickups.AddRange(healthPickups);
            allPickups.AddRange(ammoPickups);
            allPickups.AddRange(crystalPickups);
            return allPickups;
        }

        public void Initialize()
        {
            if(isInitialized) return;
            isInitialized = true;
            ItemsDatabase database = Resources.Load<ItemsDatabase>(nameof(ItemsDatabase));
            if (!database)
            {
                Debug.LogError("Items database is missing");
                return;
            }

            for (int i = 0; i < database.ItemDefinitions.Count; i++)
            {
                ItemBase item = CreateNewItem(database.ItemDefinitions[i]);
                if(item == null) continue;
                AddItemToLootTable(item);
            }
            
        }

        private ItemBase CreateNewItem(ItemData data)
        {
            if (data == null)
            {
                Debug.LogError($"{this} Item data is null");
                return null;
            }

            switch (data)
            {
                case PassiveItemData passiveItemData:
                    PassiveItem passiveItem = new (passiveItemData);
                    return passiveItem;
                
                case ActiveItemData activeItemData:
                    ActiveItem activeItem = new (activeItemData);
                    return activeItem;
                
                case MeleeWeaponData meleeWeaponData:
                    MeleeWeapon meleeWeapon = new (meleeWeaponData);
                    return meleeWeapon;
                
                case RangedWeaponData rangedWeaponData:
                    RangedWeapon rangedWeapon = new (rangedWeaponData);
                    return rangedWeapon;
            }

            Debug.LogError($"{this} Couldn't find matching type");
            return null;
        }
        
        public List<ItemBase> GetRandomItemsFromEachCategory()
        {
            List<ItemBase> selectedItems = new ();

            var rangedWeapon = GetRandomRangedWeapon(); // Get a random RangedWeapon
            if (rangedWeapon != null) selectedItems.Add(rangedWeapon);

            var meleeWeapon = GetRandomMeleeWeapon();  // Get a random MeleeWeapon
            if (meleeWeapon != null) selectedItems.Add(meleeWeapon);

            var passiveItem = GetRandomPassiveItem();  // Get a random PassiveItem
            if (passiveItem != null) selectedItems.Add(passiveItem);

            var activeItem = GetRandomActiveItem();  // Get a random ActiveItem
            if (activeItem != null) selectedItems.Add(activeItem);

            return selectedItems;
        }

        public ItemBase GetRandomItem()
        {
            return GetRandomItemsFromEachCategory().GetRandomElement<ItemBase>();
        }
        
        public List<PickupBase> GetRandomPickupsFromEachCategory()
        {
            List<PickupBase> selectedPickups = new List<PickupBase>();

            PickupBase healthPickup = GetRandomHealthPickup();
            if (healthPickup != null) selectedPickups.Add(healthPickup);

            PickupBase ammoPickup = GetRandomAmmoPickup();
            if (ammoPickup != null) selectedPickups.Add(ammoPickup);
            
            PickupBase crystalPickup = GetRandomCrystalPickup();
            if (crystalPickup != null) selectedPickups.Add(crystalPickup);

            return selectedPickups;
        }

        //TODO: Change all of this to PopRandomElement, which also deletes it from the list
        public ActiveItem GetRandomActiveItem()
        {
            return activeItems.GetRandomElement<ActiveItem>();
        }

        public PassiveItem GetRandomPassiveItem()
        {
            return passiveItems.GetRandomElement<PassiveItem>();
        }

        public MeleeWeapon GetRandomMeleeWeapon()
        {
            return meleeWeapons.GetRandomElement<MeleeWeapon>();
        }

        public RangedWeapon GetRandomRangedWeapon()
        {
            return rangedWeapons.GetRandomElement<RangedWeapon>();
        }
        
        public PickupBase GetRandomPickup() => GetRandomPickupsFromEachCategory().GetRandomElement<PickupBase>();
        public HealthPickup GetRandomHealthPickup()
        {
            return healthPickups.GetRandomElement<HealthPickup>();
        }

        public AmmoPickup GetRandomAmmoPickup()
        {
            return ammoPickups.GetRandomElement<AmmoPickup>();
        }
        
        public CrystalPickup GetRandomCrystalPickup()
        {
            return crystalPickups.GetRandomElement<CrystalPickup>();
        }
        
        public bool AddItemToLootTable(ItemBase item)
        {
           
            if(item == null) return false;
            switch (item.ItemType)
            {
                case ItemType.Passive:
                    if (!passiveItems.AddUnique(item as PassiveItem))
                        break;
                    return true;

                case ItemType.Active:
                    if (!activeItems.AddUnique(item as ActiveItem))
                        break;
                    return true;

                case ItemType.Melee:
                    if (!meleeWeapons.AddUnique(item as MeleeWeapon))
                        break;
                    return true;

                case ItemType.Ranged:
                    if (!rangedWeapons.AddUnique(item as RangedWeapon))
                        break;
                    return true;
                
                default:
                    Debug.LogError($"Item {item.ItemType} is not a valid item type");
                    return false;
            }
            
            Debug.LogError($"Item {item.ItemName} already exists in the loot table");
            return false;
        }
        
        public bool AddPickupToLootTable(PickupBase PickupBase)
        {
            if(PickupBase == null) return false;
            switch (PickupBase.PickupType)
            {
                case PickupType.Health:
                    if (!healthPickups.AddUnique(PickupBase as HealthPickup))
                        break;
                    return true;
                
                case PickupType.Ammo:
                    if (!ammoPickups.AddUnique(PickupBase as AmmoPickup))
                        break;
                    return true;
                
                case PickupType.Crystal:
                    if (!crystalPickups.AddUnique(PickupBase as CrystalPickup))
                        break;
                    return true;
                
                default:
                    Debug.LogError($"PickupBase {PickupBase.PickupType} is not a valid item type");
                    return false;
            }
            
            Debug.LogError($"PickupBase {PickupBase.PickupName} already exists in the loot table");
            return false;
        }
    }
}
    