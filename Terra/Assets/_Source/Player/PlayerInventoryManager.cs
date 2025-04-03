using System;
using System.Collections.Generic;
using Terra.Core.Generics;
using NaughtyAttributes;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Interfaces;
using Terra.Itemization.Items;
using Terra.Itemization.Items.Definitions;
using UnityEngine;

namespace Terra.Player
{
    public class PlayerInventoryManager : MonoBehaviourSingleton<PlayerInventoryManager>
    {
        [Foldout("References")] [SerializeField] StartingInventoryData startingInventoryData;
        
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<MeleeWeapon> meleeWeaponSlot = new ();
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<RangedWeapon> rangedWeaponSlot = new();
        [Foldout("Debug"), SerializeField,  ReadOnly] private ItemSlot<ActiveItem> activeItemSlot = new ();
        
        private ItemSlotBase[] itemSlots;
        
        [Foldout("Debug"), SerializeField, ReadOnly]private List<PassiveItem> passiveItems;

        private bool isInitialized = false;
        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        //TODO:Add removing item from EQ and spawning ItemContainer with the item through some kind of LootManager.Instance.SpawnItem(item, pos)
        void Initialize()
        {
            if(isInitialized) return;
            isInitialized = true;
            meleeWeaponSlot = new();
            rangedWeaponSlot = new();
            activeItemSlot = new();
            passiveItems = new();
            
            if (startingInventoryData != null)
            {
                meleeWeaponSlot.Equip(startingInventoryData.startingMelee);
                rangedWeaponSlot.Equip(startingInventoryData.startingRanged);
                activeItemSlot.Equip(startingInventoryData.startingActive);
                startingInventoryData.startingPassiveItems.CopyTo(passiveItems.ToArray());
            }
            itemSlots = new ItemSlotBase[Enum.GetValues(typeof(ItemType)).Length];
            itemSlots[(int)ItemType.Melee] = meleeWeaponSlot ;
            itemSlots[(int)ItemType.Ranged] = rangedWeaponSlot;
            itemSlots[(int)ItemType.Active] = activeItemSlot;
        }
        

        public bool TryToEquipItem<TItem>(TItem newItem)
        where TItem : ItemBase
        {
            if(newItem is not IEquipable equipable) return false;
            if(!CanEquipItem(newItem)) return false;
            
            ItemType itemType = newItem.ItemType;

            
            if (itemType == ItemType.Passive)
            {
                equipable.OnEquip();
                passiveItems.Add(newItem as PassiveItem);
                return true;
            }
            
            ItemSlotBase itemSlot = GetItemSlotBase(itemType);

            if (itemSlot.IsSlotTaken)
            {
                return itemSlot.SwapNonGeneric(newItem);
            }
            return itemSlot.EquipNonGeneric(newItem);
        }
        
        private ItemSlotBase GetItemSlotBase(ItemType itemType)
        {
           return itemSlots[(int)itemType];
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
        
        public List<PassiveItem> GetPassiveItems() => passiveItems;

    }
    
}