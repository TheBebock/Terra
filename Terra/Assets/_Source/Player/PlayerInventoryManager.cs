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
using UnityEngine.InputSystem;

namespace Terra.Player
{
    public class PlayerInventoryManager : MonoBehaviourSingleton<PlayerInventoryManager>
    {
        [SerializeField, Expandable] StartingInventoryData _startingInventoryData;
        
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<MeleeWeapon> _meleeWeaponSlot = new ();
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<RangedWeapon> _rangedWeaponSlot = new();
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<ActiveItem> _activeItemSlot = new ();
        [Foldout("Debug"), SerializeField, ReadOnly] private List<PassiveItem> _passiveItems = new();

        private ItemSlotBase[] _itemSlots;
        public List<PassiveItem> GetPassiveItems => _passiveItems;
        public ActiveItem ActiveItem => _activeItemSlot.EquippedItem;
        public MeleeWeapon MeleeWeapon => _meleeWeaponSlot.EquippedItem;
        public RangedWeapon RangedWeapon => _rangedWeaponSlot.EquippedItem;

        public event Action<PassiveItem> OnPassiveItemAdded;
        public event Action<ActiveItem> OnActiveItemChanged;
        public event Action<MeleeWeapon> OnMeleeWeaponChanged;
        public event Action<RangedWeapon> OnRangedWeaponChanged;


        protected override void Awake()
        {
            base.Awake();
            
            InitEquipmentSlots();

            // Create fast access array to created item slots
            _itemSlots = new ItemSlotBase[Enum.GetValues(typeof(ItemType)).Length];
            _itemSlots[(int)ItemType.Melee] = _meleeWeaponSlot ;
            _itemSlots[(int)ItemType.Ranged] = _rangedWeaponSlot;
            _itemSlots[(int)ItemType.Active] = _activeItemSlot;
            
            // Attach listeners
            ItemSlotBase.OnItemRemoved += DropItemOnGround;
        }

        private void InitEquipmentSlots()
        {
            // Create item slots
            _meleeWeaponSlot = new();
            _rangedWeaponSlot = new();
            _activeItemSlot = new();
            _passiveItems = new();
   
            
            
            // Equip starting items
            if (_startingInventoryData != null)
            {
                _meleeWeaponSlot.Equip(_startingInventoryData.startingMelee);
                _rangedWeaponSlot.Equip(_startingInventoryData.startingRanged);
                _activeItemSlot.Equip(_startingInventoryData.startingActive);

                for (int i = 0; i < _startingInventoryData.startingPassiveItems.Count; i++)
                {
                    _passiveItems.Add(_startingInventoryData.startingPassiveItems[i]);
                }
                
                for (int i = 0; i < _passiveItems.Count; i++)
                {
                    _passiveItems[i].OnEquip();
                }
            }

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
                _passiveItems.Add(passiveItem);
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
           return _itemSlots[(int)itemType];
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
            for (int i = 0; i < _itemSlots.Length; i++) 
            {
                if (_itemSlots[i] is not ItemSlot<TItem> { IsSlotTaken: true } typedSlot) continue;
                item = typedSlot.EquippedItem;
                return true;
            }
            item = null;
            return false;
        }
        
        public ItemSlot<TItem> GetItemSlotBySlotType<TItem>()
            where TItem : ItemBase
        {
            for (int i = 0; i < _itemSlots.Length; i++)
            {
                if (_itemSlots[i] is ItemSlot<TItem> slot)
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

        private void OnValidate()
        {
            InitEquipmentSlots();
        }

        private void OnDrawGizmos()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            //TODO: Change to camera manager
            Ray ray = Camera.main.ScreenPointToRay( mousePosition );
            Plane plane = new Plane(Vector3.up, transform.position);

            // Raycast get point where player clicked on screen while we use perspective camera
            if (!plane.Raycast(ray, out float enter)) return;
            
            
            Vector3 worldClickPosition = ray.GetPoint(enter);
            Vector3 direction = (worldClickPosition - transform.position).normalized;
            Vector3 attackPosition = transform.position + direction * MeleeWeapon.Data.range;
            Quaternion targetRotation = Quaternion.LookRotation(attackPosition - transform.position);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition, MeleeWeapon.Data.sphereHitboxRadius);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(attackPosition, MeleeWeapon.Data.hitboxSize);
        }
    }
    
}