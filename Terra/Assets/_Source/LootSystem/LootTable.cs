using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Terra.Itemization;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items;
using Terra.Itemization.Items.Definitions;
using Terra.Itemization.Pickups;
using UnityEditor;
using UnityEngine;

namespace Terra.LootSystem
{
    /// <summary>
    /// Contains all possible items and pickup that can be dropped in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "LootTable", menuName = "TheBebocks/LootTable")]
    public class LootTable : ScriptableSingleton<LootTable>
    {
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
        
        public List<Pickup> GetAllPickups()
        {
            List<Pickup> allPickups = new();
            allPickups.AddRange(healthPickups);
            allPickups.AddRange(ammoPickups);
            allPickups.AddRange(crystalPickups);
            return allPickups;
        }

        public void Initialize()
        {
            
            ItemsDatabase database = ItemsDatabase.instance;
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
        
        public List<Pickup> GetRandomPickupsFromEachCategory()
        {
            List<Pickup> selectedPickups = new List<Pickup>();

            Pickup healthPickup = GetRandomHealthPickup();
            if (healthPickup != null) selectedPickups.Add(healthPickup);

            Pickup ammoPickup = GetRandomAmmoPickup();
            if (ammoPickup != null) selectedPickups.Add(ammoPickup);
            
            Pickup crystalPickup = GetRandomCrystalPickup();
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
        
        public Pickup GetRandomPickup() => GetRandomPickupsFromEachCategory().GetRandomElement<Pickup>();
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
        
        public static bool AddItemToLootTable(ItemBase item)
        {
            if (instance == null)
            {
                Debug.LogError($"{nameof(LootTable)} Does not exist");
                return false;
            }
            if(item == null) return false;
            switch (item.ItemType)
            {
                case ItemType.Passive:
                    if (!instance.passiveItems.AddUnique(item as PassiveItem))
                        break;
                    return true;

                case ItemType.Active:
                    if (!instance.activeItems.AddUnique(item as ActiveItem))
                        break;
                    return true;

                case ItemType.Melee:
                    if (!instance.meleeWeapons.AddUnique(item as MeleeWeapon))
                        break;
                    return true;

                case ItemType.Ranged:
                    if (!instance.rangedWeapons.AddUnique(item as RangedWeapon))
                        break;
                    return true;
                
                default:
                    Debug.LogError($"Item {item.ItemType} is not a valid item type");
                    return false;
            }
            
            Debug.LogError($"Item {item.ItemName} already exists in the loot table");
            return false;
        }
        
        public static bool AddPickupToLootTable(Pickup pickup)
        {
            if(pickup == null) return false;
            switch (pickup.PickupType)
            {
                case PickupType.Health:
                    if (!instance.healthPickups.AddUnique(pickup as HealthPickup))
                        break;
                    return true;
                
                case PickupType.Ammo:
                    if (!instance.ammoPickups.AddUnique(pickup as AmmoPickup))
                        break;
                    return true;
                
                case PickupType.Crystal:
                    if (!instance.crystalPickups.AddUnique(pickup as CrystalPickup))
                        break;
                    return true;
                
                default:
                    Debug.LogError($"Pickup {pickup.PickupType} is not a valid item type");
                    return false;
            }
            
            Debug.LogError($"Pickup {pickup.PickupName} already exists in the loot table");
            return false;
        }
    }
}
    