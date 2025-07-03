using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Abstract.Definitions;
using Terra.Extensions;
using Terra.Itemization;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Itemization.Items;
using Terra.Itemization.Pickups;
using UnityEngine;
using UnityEngine.UIElements;

namespace Terra.LootSystem
{

    [Serializable]
    internal struct StatusDropData
    {
        [Range(1,100)] public float dropChance;
        public StatusEffectData statusEffect;
    }
    
    [Serializable]
    internal struct ActionDropData
    {
        [Range(1,100)] public float dropChance;
        public ActionEffectData actionEffect;
    }
    /// <summary>
    /// Contains all possible items and PickupBase that can be dropped in the game.
    /// </summary>
    [Serializable]
    public class LootTable
    {
        
        [Header("Items - Assigned Automatically")]
        [SerializeField] private List<ActiveItem> _activeItems = new (); 
        [SerializeField] private List<PassiveItem> _passiveItems = new ();
        [SerializeField] private List<MeleeWeapon> _meleeWeapons = new ();
        [SerializeField] private List<RangedWeapon> _rangedWeapons = new ();
        
        [Header("Pickups - Assigned Manually")]
        [SerializeField] private List<HealthPickup> _healthPickups = new ();
        [SerializeField] private List<AmmoPickup> _ammoPickups = new ();
        [SerializeField] private List<CrystalPickup> _crystalPickups = new ();
        [Header("Effects - Assigned Manually")]
        [SerializeField] private List<StatusDropData> _statusEffects = new ();
        [SerializeField] private List<ActionDropData> _actionEffects = new ();
        
        [Foldout("Debug"), ReadOnly] [SerializeField] private List<StatusDropData> _statusEffectsPerGame = new ();
        [Foldout("Debug"), ReadOnly] [SerializeField] private List<ActionDropData> _actionEffectsPerGame = new ();
        private bool _isInitialized;
        
        public int PassiveItemsCount => _passiveItems.Count;
        public int MeleeWeaponsCount => _meleeWeapons.Count;
        public int RangedWeaponsCount => _rangedWeapons.Count;
        public int StatusEffectsCount => _statusEffectsPerGame.Count;
        public int ActionEffectsCount => _actionEffectsPerGame.Count;

        public List<ItemBase> GetAllItems()
        {
            List<ItemBase> allItems = new ();
            allItems.AddRange(_activeItems);
            allItems.AddRange(_passiveItems);
            allItems.AddRange(_meleeWeapons);
            allItems.AddRange(_rangedWeapons);
            return allItems;
        }
        
        public List<PickupBase> GetAllPickups()
        {
            List<PickupBase> allPickups = new();
            allPickups.AddRange(_healthPickups);
            allPickups.AddRange(_ammoPickups);
            allPickups.AddRange(_crystalPickups);
            return allPickups;
        }

        public void Initialize()
        {
            if(_isInitialized) return;
            _isInitialized = true;
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
            _healthPickups = _healthPickups.OrderByDescending(pickup => pickup.Data.dropRateChance).ToList();
            _ammoPickups = _ammoPickups.OrderByDescending(pickup => pickup.Data.dropRateChance).ToList();
            _crystalPickups = _crystalPickups.OrderByDescending(pickup => pickup.Data.dropRateChance).ToList();
            
            _statusEffects = _statusEffects.OrderByDescending(status => status.dropChance).ToList();
            _actionEffects = _actionEffects.OrderByDescending(status => status.dropChance).ToList();
            
            _statusEffectsPerGame.AddRange(_statusEffects);
            _actionEffectsPerGame.AddRange(_actionEffects);
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

#region POP_RANDOM
        public StatusEffectData PopRandomStatusEffect()
        {
            return _statusEffectsPerGame.PopRandomElement().statusEffect;
        }
        
        public ActionEffectData PopRandomActionEffect()
        {
            return _actionEffectsPerGame.PopRandomElement().actionEffect;
        }
        public ActiveItem PopRandomActiveItem()
        {
            return _activeItems.PopRandomElement();
        }

        public PassiveItem PopRandomPassiveItem()
        {
            return _passiveItems.PopRandomElement();
        }

        public MeleeWeapon PopRandomMeleeWeapon()
        {
            return _meleeWeapons.PopRandomElement();
        }

        public RangedWeapon PopRandomRangedWeapon()
        {
            return _rangedWeapons.PopRandomElement();
        }

        public List<ItemBase> PopRandomItemsFromEachCategory()
        {
            List<ItemBase> selectedItems = new();

            var rangedWeapon = PopRandomRangedWeapon();
            if (rangedWeapon != null) selectedItems.Add(rangedWeapon);

            var meleeWeapon = PopRandomMeleeWeapon();
            if (meleeWeapon != null) selectedItems.Add(meleeWeapon);

            var passiveItem = PopRandomPassiveItem();
            if (passiveItem != null) selectedItems.Add(passiveItem);

            var activeItem = PopRandomActiveItem();
            if (activeItem != null) selectedItems.Add(activeItem);

            return selectedItems;
        }
#endregion

#region GET_RANDOM

        public ItemBase GetRandomItem()
        {
            return GetRandomItemsFromEachCategory().GetRandomElement<ItemBase>();
        }

        public List<ItemBase> GetRandomItemsFromEachCategory()
        {
            List<ItemBase> selectedItems = new ();

            var rangedWeapon = GetRandomRangedWeapon();
            if (rangedWeapon != null) selectedItems.Add(rangedWeapon);

            var meleeWeapon = GetRandomMeleeWeapon();
            if (meleeWeapon != null) selectedItems.Add(meleeWeapon);

            var passiveItem = GetRandomPassiveItem();
            if (passiveItem != null) selectedItems.Add(passiveItem);

            var activeItem = GetRandomActiveItem();
            if (activeItem != null) selectedItems.Add(activeItem);

            return selectedItems;
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

        public StatusEffectData GetRandomStatusEffect()
        {
            return _statusEffectsPerGame.GetRandomElement<StatusDropData>().statusEffect;
        }

        public ActionEffectData GetRandomActionEffect()
        {
            return _actionEffectsPerGame.GetRandomElement<ActionDropData>().actionEffect;
        }
        public ActiveItem GetRandomActiveItem()
        {
            return _activeItems.GetRandomElement<ActiveItem>();
        }
        public PassiveItem GetRandomPassiveItem()
        {
            return _passiveItems.GetRandomElement<PassiveItem>();
        }

        public MeleeWeapon GetRandomMeleeWeapon()
        {
            return _meleeWeapons.GetRandomElement<MeleeWeapon>();
        }

        public RangedWeapon GetRandomRangedWeapon()
        {
            return _rangedWeapons.GetRandomElement<RangedWeapon>();
        }


        public PickupBase GetRandomPickup() => GetRandomPickupsFromEachCategory().GetRandomElement<PickupBase>();
        public HealthPickup GetRandomHealthPickup()
        {
            return GetPickup(_healthPickups) as HealthPickup;
        }

        public AmmoPickup GetRandomAmmoPickup()
        {
            return GetPickup(_ammoPickups) as AmmoPickup;
        }
        
        public CrystalPickup GetRandomCrystalPickup()
        {
            return GetPickup(_crystalPickups) as CrystalPickup;
        }

        private PickupBase GetPickup(IEnumerable<PickupBase> pickups)
        {
            float randomChance = UnityEngine.Random.Range(0f, 100f);
            List<PickupBase> pickupBases = pickups.ToList();
            for (int i = pickupBases.Count-1; i >= 0 ; i--)
            {
                if (randomChance <= pickupBases[i].DropRate && pickupBases[i].DropRate > 0f)
                {
                    return pickupBases[i];
                }
            }
            return pickupBases[0];
        }
#endregion 

        public bool AddItemToLootTable(ItemBase item)
        {
           
            if(item == null) return false;
            switch (item.ItemType)
            {
                case ItemType.Passive:
                    if (!_passiveItems.AddUnique(item as PassiveItem))
                        break;
                    return true;

                case ItemType.Active:
                    if (!_activeItems.AddUnique(item as ActiveItem))
                        break;
                    return true;

                case ItemType.Melee:
                    if (!_meleeWeapons.AddUnique(item as MeleeWeapon))
                        break;
                    return true;

                case ItemType.Ranged:
                    if (!_rangedWeapons.AddUnique(item as RangedWeapon))
                        break;
                    return true;
                
                default:
                    Debug.LogError($"Item {item.ItemType} is not a valid item type");
                    return false;
            }
            
            Debug.LogError($"Item {item.ItemName} already exists in the loot table");
            return false;
        }
        
        public bool AddPickupToLootTable(PickupBase pickupBase)
        {
            if(pickupBase == null) return false;
            switch (pickupBase.PickupType)
            {
                case PickupType.Health:
                    if (!_healthPickups.AddUnique(pickupBase as HealthPickup))
                        break;
                    return true;
                
                case PickupType.Ammo:
                    if (!_ammoPickups.AddUnique(pickupBase as AmmoPickup))
                        break;
                    return true;
                
                case PickupType.Crystal:
                    if (!_crystalPickups.AddUnique(pickupBase as CrystalPickup))
                        break;
                    return true;
                
                default:
                    Debug.LogError($"PickupBase {pickupBase.PickupType} is not a valid item type");
                    return false;
            }
            
            Debug.LogError($"PickupBase {pickupBase.PickupName} already exists in the loot table");
            return false;
        }

#region REMOVES


        public void RemoveStatusEffect(StatusEffectData statusEffect)
        {
            int index = _statusEffectsPerGame.FindIndex(statusDrop => statusDrop.statusEffect == statusEffect);
            if (index >= 0)
                _statusEffectsPerGame.RemoveAt(index);
        }

        public void RemoveActionEffect(ActionEffectData actionEffect)
        {
            int index = _actionEffectsPerGame.FindIndex(actionDrop => actionDrop.actionEffect == actionEffect);
            if(index >= 0)
                _statusEffectsPerGame.RemoveAt(index);
        }
        public void RemoveActiveItem(ActiveItem activeItem)
        {
            _activeItems.RemoveElement(activeItem);
        }
        public void RemovePassiveItem(PassiveItem passiveItem)
        {
            _passiveItems.RemoveElement(passiveItem);
        }

        public void RemoveMeleeWeapon(MeleeWeapon meleeWeapon)
        {
            _meleeWeapons.RemoveElement(meleeWeapon);
        }

        public void RemoveRangedWeapon(RangedWeapon rangedWeapon)
        {
            _rangedWeapons.RemoveElement(rangedWeapon);
        }

#endregion
    }
}
    