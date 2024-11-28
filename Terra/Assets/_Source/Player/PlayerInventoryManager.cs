using System;
using System.Collections.Generic;
using Core.Generics;
using Inventory;
using Inventory.Abstracts;
using Inventory.Items;
using Inventory.Items.Definitions;
using NaughtyAttributes;
using UnityEngine;

namespace Player
{
    public class PlayerInventoryManager : MonoBehaviourSingleton<PlayerInventoryManager>
    {
        [Foldout("References")] [SerializeField] StartingInventoryData _startingInventoryData;
        //TOOD: Maybe add MessagePack to serialize classes that don't inherit SO or MB
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

        void Initialize()
        {
            if(isInitialized) return;
            if (_startingInventoryData != null)
            {
                meleeWeaponSlot.Equip(_startingInventoryData.startingMelee);
                rangedWeaponSlot.Equip(_startingInventoryData.startingRanged);
                activeItemSlot.Equip(_startingInventoryData.startingActive);
                _startingInventoryData.startingPassiveItems.CopyTo(passiveItems.ToArray());
            }
            isInitialized = true;
            meleeWeaponSlot = new();
            rangedWeaponSlot = new();
            activeItemSlot = new();
            passiveItems = new();
            itemSlots = new ItemSlotBase[Enum.GetValues(typeof(ItemType)).Length];
            itemSlots[(int)ItemType.Melee] = meleeWeaponSlot;
            itemSlots[(int)ItemType.Ranged] = rangedWeaponSlot;
            itemSlots[(int)ItemType.Active] = activeItemSlot;
        }
        

        public bool TryToEquipItem(Item newItem)
        {
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