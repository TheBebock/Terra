using System;
using System.Collections.Generic;
using Terra.Core.Generics;
using NaughtyAttributes;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items;
using Terra.Itemization.Items.Definitions;
using UnityEngine;

namespace Player
{
    public class PlayerInventoryManager : MonoBehaviourSingleton<PlayerInventoryManager>
    {
        [Foldout("References")] [SerializeField] StartingInventoryData startingInventoryData;
        
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<MeleeWeapon> meleeWeaponSlot = new ItemSlot<MeleeWeapon>();
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<RangedWeapon> rangedWeaponSlot = new ItemSlot<RangedWeapon>();
        [Foldout("Debug"), SerializeField,  ReadOnly] private ItemSlot<ActiveItem> activeItemSlot = new ItemSlot<ActiveItem>();
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
            itemSlots[(int)ItemType.Melee] = meleeWeaponSlot;
            itemSlots[(int)ItemType.Ranged] = rangedWeaponSlot;
            itemSlots[(int)ItemType.Active] = activeItemSlot;
        }
        

        public bool TryToEquipItem(Item newItem)
        {
            if(!CanEquipItem(newItem)) return false;
            
            ItemType itemType = newItem.itemType;

            
            if (itemType == ItemType.Passive)
            {
                newItem.OnEquip();
                passiveItems.Add(newItem as PassiveItem);
                return true;
            }
            
            ItemSlotBase itemSlot = GetItemSlotBase(itemType);

            if (itemSlot.IsSlotTaken)
            {
                return itemSlot.Swap(newItem);
            }
            return itemSlot.Equip(newItem);
        }
        
        private ItemSlotBase GetItemSlotBase(ItemType itemType) => itemSlots[(int)itemType];
        
        public bool CanEquipItem(Item newItem) 
        {
            if (newItem is PassiveItem) return true;
            ItemSlotBase itemSlot = GetItemSlotBase(newItem.itemType);
            return CanEquipItem(itemSlot);
        }
        
        bool CanEquipItem(ItemSlotBase itemSlot)
        {
            if(itemSlot == null) return false;

            return itemSlot.CanEquip();
        }
        
        /// <summary>
        /// Method returns item based on given type, if there is one.
        /// </summary>
        /// <returns>Returns item of type Item, that then has to be cast.</returns>
        public bool TryToGetItemByItemType<T>(out Item item) 
            where T : Item
        {
            for (int i = 0; i < itemSlots.Length; i++) 
            {
                if (itemSlots[i] is not ItemSlot<T> { IsSlotTaken: true } typedSlot) continue;
                item = typedSlot.EquippedItem;
                return true;
            }
            item = null;
            return false;
        }
        
        public ItemSlot<T> GetItemSlotBySlotType<T>()
            where T : Item
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i] is ItemSlot<T> slot)
                {
                    return slot;
                }
            }
            return null;
        }
        
        public List<PassiveItem> GetPassiveItems() => passiveItems;

    }
    
}