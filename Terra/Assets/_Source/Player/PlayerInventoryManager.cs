using System;
using System.Collections.Generic;
using System.Drawing.Design;
using Core.Generics;
using Inventory;
using Inventory.Abstracts;
using Inventory.Items;
using Inventory.Items.Definitions;
using NaughtyAttributes;
using OdinSerializer;
using UnityEngine;

namespace Player
{
    public class PlayerInventoryManager : MonoBehaviourSingleton<PlayerInventoryManager>
    {
        //TOOD: Maybe add MessagePack to serialize classes that don't inherit SO or MB
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<MeleeWeapon> meleeWeaponSlot = new ItemSlot<MeleeWeapon>();
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<RangedWeapon> rangedWeaponSlot = new ItemSlot<RangedWeapon>();
        [Foldout("Debug"), SerializeField,  ReadOnly] private ItemSlot<ActiveItem> activeItemSlot = new ItemSlot<ActiveItem>();
        private ItemSlotBase[] itemSlots;
        
        [Foldout("Debug"), SerializeField, ReadOnly]private List<Item> passiveItems;

        private bool isInitialized = false;
        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        void Initialize()
        {
            if(isInitialized) return;
            
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
                newItem.Equip();
                passiveItems.Add(newItem);
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
        
        public ItemSlot<T> GetItemSlotByType<T>()
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
        
        public bool CanEquipItem<T>(T newItem)
        where T : Item
        {
            if (newItem is PassiveItem) return true;
            ItemSlot<T> itemSlot = GetItemSlotByType<T>();
            return CanEquipItem(itemSlot);
        }
        
        bool CanEquipItem<T>(ItemSlot<T> itemSlot)
            where T : Item
        {
            if (!itemSlot.CanEquip())
            {
                return false;
            }
            return true;
        }
        
        public bool TryToGetItemByItemSlotType<T>(out Item item) 
            where T : Item
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i] is ItemSlot<T> typedSlot)
                {
                    item = typedSlot.EquippedItem;
                    return true;
                }
            }
            item = null;
            return false;
        }
        
        
        
        public List<Item> GetPassiveItems() => passiveItems;

    }
    
}