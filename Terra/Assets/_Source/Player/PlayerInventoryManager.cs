using System;
using System.Collections.Generic;
using Terra.Core.Generics;
using NaughtyAttributes;
using Terra.ID;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Interfaces;
using Terra.Itemization.Items;
using Terra.Itemization.Items.Definitions;
using Terra.Managers;
using UnityEngine;

namespace Terra.Player
{
    public class PlayerInventoryManager : MonoBehaviourSingleton<PlayerInventoryManager>
    {
        [Foldout("References")] [SerializeField] StartingInventoryData startingInventoryData;
        
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<MeleeWeapon> meleeWeaponSlot = new ();
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<RangedWeapon> rangedWeaponSlot = new();
        [Foldout("Debug"), SerializeField,  ReadOnly] private ItemSlot<ActiveItem> activeItemSlot = new ();
        
        [Foldout("Debug"), SerializeField, ReadOnly]private List<PassiveItem> passiveItems = new();

        private ItemSlotBase[] itemSlots;
        public List<PassiveItem> GetPassiveItems => passiveItems;
        public ActiveItem ActiveItem => activeItemSlot.EquippedItem;
        public MeleeWeapon MeleeWeapon => meleeWeaponSlot.EquippedItem;
        public RangedWeapon RangedWeapon => rangedWeaponSlot.EquippedItem;

        public event Action<PassiveItem> OnPassiveItemAdded;
        public event Action<ActiveItem> OnActiveItemChanged;
        public event Action<MeleeWeapon> OnMeleeWeaponChanged;
        public event Action<RangedWeapon> OnRangedWeaponChanged;


        protected override void Awake()
        {
            base.Awake();
            // Create item slots
            meleeWeaponSlot = new();
            rangedWeaponSlot = new();
            activeItemSlot = new();
            passiveItems = new();
   
            
            
            // Equip starting items
            if (startingInventoryData != null)
            {
                meleeWeaponSlot.Equip(startingInventoryData.startingMelee);
                rangedWeaponSlot.Equip(startingInventoryData.startingRanged);
                activeItemSlot.Equip(startingInventoryData.startingActive);

                for (int i = 0; i < startingInventoryData.startingPassiveItems.Count; i++)
                {
                    passiveItems.Add(startingInventoryData.startingPassiveItems[i]);
                }
                
                for (int i = 0; i < passiveItems.Count; i++)
                {
                    passiveItems[i].OnEquip();
                }
            }
            
            // Create fast access array to created item slots
            itemSlots = new ItemSlotBase[Enum.GetValues(typeof(ItemType)).Length];
            itemSlots[(int)ItemType.Melee] = meleeWeaponSlot ;
            itemSlots[(int)ItemType.Ranged] = rangedWeaponSlot;
            itemSlots[(int)ItemType.Active] = activeItemSlot;
            
            // Attach listeners
            ItemSlotBase.OnItemRemoved += DropItemOnGround;
        }

        private void DropItemOnGround(ItemBase item)
        {
            LootManager.Instance?.SpawnItem(item, PlayerManager.Instance.CurrentPosition);
        }

        public bool TryToEquipItem<TItem>(TItem newItem)
        where TItem : ItemBase
        {
            if(newItem is not IEquipable equipable) return false;
            if(!CanEquipItem(newItem)) return false;
            
            ItemType itemType = newItem.ItemType;

            
            if (newItem is PassiveItem passiveItem)
            {
                passiveItem.OnEquip();
                passiveItems.Add(passiveItem);
                OnPassiveItemAdded?.Invoke(passiveItem);
                return true;
            }
            
            ItemSlotBase itemSlot = GetItemSlotBase(itemType);

            if (itemSlot.IsSlotTaken)
            {
                return itemSlot.SwapNonGeneric(newItem);
            }

            if (itemSlot.EquipNonGeneric(newItem))
            {
                InvokeItemChangedEvent(newItem);
                return true;
            }
            return false;
        }
        
        
        
        private ItemSlotBase GetItemSlotBase(ItemType itemType)
        {
           return itemSlots[(int)itemType];
        }

        private void InvokeItemChangedEvent(ItemBase item)
        {
            switch (item)
            {
                case ActiveItem activeItem:
                    OnActiveItemChanged?.Invoke(activeItem);    
                    break;
                case MeleeWeapon meleeWeapon:
                    OnMeleeWeaponChanged?.Invoke(meleeWeapon);
                    break;
                case RangedWeapon ranged:
                    OnRangedWeaponChanged?.Invoke(ranged);
                    break;
            }
        }
        
        /// <summary>
        /// Checks if item can be equipped to slot
        /// </summary>
        public bool CanEquipItem<TItem>(TItem newItem)
        where TItem : ItemBase
        {
            if (newItem is PassiveItem) return true;
            ItemSlotBase itemSlot = GetItemSlotBase(newItem.ItemType);
            return CanEquipItem(itemSlot);
        }
        
        
        /// <summary>
        /// Checks is slot available
        /// </summary>
        bool CanEquipItem(ItemSlotBase itemSlot)
        {
            // Null check
            if(itemSlot == null) return false;
            
            // Check can item be equipped to slot
            return itemSlot.CanEquip();
            
        }
        
        /// <summary>
        /// Method returns item based on given type, if there is one.
        /// </summary>
        /// <returns>Returns item of type Item, that then has to be cast.</returns>
        public bool TryToGetItemByItemType<TItem>(out TItem item) 
            where TItem : ItemBase
        {
            for (int i = 0; i < itemSlots.Length; i++) 
            {
                if (itemSlots[i] is not ItemSlot<TItem> { IsSlotTaken: true } typedSlot) continue;
                item = typedSlot.EquippedItem;
                return true;
            }
            item = null;
            return false;
        }
        
        public ItemSlot<TItem> GetItemSlotBySlotType<TItem>()
            where TItem : ItemBase
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i] is ItemSlot<TItem> slot)
                {
                    return slot;
                }
            }
            return null;
        }
        

        protected override void CleanUp()
        {
            base.CleanUp();
            ItemSlotBase.OnItemRemoved -= DropItemOnGround;
        }
    }
    
}