using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inventory.Pickups;
using NaughtyAttributes;
using OdinSerializer.Utilities;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items;
using Terra.Itemization.Items.Definitions;
using UnityEngine;

namespace Terra.LootSystem
{
    [CreateAssetMenu(fileName = "LootTable", menuName = "TheBebocks/LootTable")]
    public class LootTable : ScriptableObject
    {
        [Foldout("References")][SerializeField] private ItemContainer P_ItemContainer;
        [Foldout("References")][SerializeField] private PickupContainer P_PickupContainer;
        

        [SerializeField] private List<ActiveItem> activeItems = new (); 
        [SerializeField] private List<PassiveItem> passiveItems = new ();
        [SerializeField] private List<MeleeWeapon> meleeWeapons = new ();
        [SerializeField] private List<RangedWeapon> rangedWeapons = new ();
        
        [SerializeField] private List<HealthPickup> healthPickups = new ();
        [SerializeField] private List<AmmoPickup> ammoPickups = new ();
        [SerializeField] private List<CrystalPickup> crystalPickups = new ();

        private List<Item> GetAllItems()
        {
            List<Item> allItems = new List<Item>();
            allItems.AddRange(activeItems);
            allItems.AddRange(passiveItems);
            allItems.AddRange(meleeWeapons);
            allItems.AddRange(rangedWeapons);
            return allItems;
        }
        
        private List<Pickup> GetAllPickups()
        {
            List<Pickup> allPickups = new();
            allPickups.AddRange(healthPickups);
            allPickups.AddRange(ammoPickups);
            allPickups.AddRange(crystalPickups);
            return allPickups;
        }
        public List<Item> GetRandomItemsFromEachCategory()
        {
            List<Item> selectedItems = new List<Item>();

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
    }
}
    