using System;
using System.Collections.Generic;
using Terra.Core.Generics;
using NaughtyAttributes;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Interfaces;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Itemization.Interfaces;
using Terra.Itemization.Items;
using Terra.Itemization.Items.Definitions;
using Terra.Managers;
using UnityEngine;

namespace Terra.Player
{
    public class PlayerInventoryManager : MonoBehaviourSingleton<PlayerInventoryManager>, IAttachListeners, IWithSetUp
    {
        [SerializeField, Expandable] StartingInventoryData _startingInventoryData;
        [Foldout("Debug"), SerializeField, ReadOnly]private int _currentAmmo =20;
        [Foldout("Debug"), SerializeField, ReadOnly]private int _maxAmmo;
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<MeleeWeapon> _meleeWeaponSlot = new ();
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<RangedWeapon> _rangedWeaponSlot = new();
        [Foldout("Debug"), SerializeField, ReadOnly] private ItemSlot<ActiveItem> _activeItemSlot = new ();
        [Foldout("Debug"), SerializeField, ReadOnly] private List<PassiveItem> _passiveItems = new();

        private ItemSlotBase[] _itemSlots;       
        public int CurrentAmmo => _currentAmmo;
        public int MaxAmmo => _maxAmmo;
        public List<PassiveItem> GetPassiveItems => _passiveItems;
        public ActiveItem ActiveItem => _activeItemSlot.EquippedItem;
        public MeleeWeapon MeleeWeapon => _meleeWeaponSlot.EquippedItem;
        public RangedWeapon RangedWeapon => _rangedWeaponSlot.EquippedItem;
        public event Action<PassiveItem> OnPassiveItemAdded;
        public event Action<ActiveItem> OnActiveItemChanged;
        public event Action<MeleeWeapon> OnMeleeWeaponChanged;
        public event Action<RangedWeapon> OnRangedWeaponChanged;
        public event Action<int> OnCurrentAmmoChanged;  
        public event Action<int> OnMaxAmmoChanged;  
        
        private OnWeaponsChangedEvent _onWeaponsChanged;

        protected override void Awake()
        {
            base.Awake();
            
            _onWeaponsChanged = new OnWeaponsChangedEvent();
            
            InitEquipmentSlots();

            // Create fast access array to created item slots
            _itemSlots = new ItemSlotBase[Enum.GetValues(typeof(ItemType)).Length];
            _itemSlots[(int)ItemType.Melee] = _meleeWeaponSlot ;
            _itemSlots[(int)ItemType.Ranged] = _rangedWeaponSlot;
            _itemSlots[(int)ItemType.Active] = _activeItemSlot;
            
            // Attach listeners
            ItemSlotBase.OnItemRemoved += DropItemOnGround;
            
            TryEquipStartingInventory();
        }

        public void SetUp()
        {
            // This initializes ammo display
            SetCurrentAmmo(_startingInventoryData.startingAmmo);
            SetMaxAmmo(_maxAmmo);
        }

        private void TryEquipStartingInventory()
        {
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
        private void InitEquipmentSlots()
        {
            // Create item slots
            _meleeWeaponSlot = new();
            _rangedWeaponSlot = new();
            _activeItemSlot = new();
            _passiveItems = new();
        }
        
        public void AttachListeners()
        {
            EventsAPI.Register<OnPlayerRangeAttackPerformedEvent>(OnRangedAttack);
        }
        
        private void DropItemOnGround(ItemBase item)
        {
            LootManager.Instance?.SpawnItemContainer(item, PlayerManager.Instance.CurrentPosition);
        }

        public bool TryToEquipItem<TItem>(TItem newItem)
        where TItem : ItemBase
        {
            if(newItem is not IEquipable) return false;
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

                if (itemSlot.SwapNonGeneric(newItem))
                {
                    InvokeItemChangedEvent(newItem);
                    return true;
                }
            }

            if (!itemSlot.EquipNonGeneric(newItem)) return false;
            
            InvokeItemChangedEvent(newItem);
            return true;
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
                    _onWeaponsChanged.itemType = WeaponType.Melee;
                    _onWeaponsChanged.weaponSprite = meleeWeapon.ItemIcon;
                    EventsAPI.Invoke(ref _onWeaponsChanged);
                    break;
                case RangedWeapon ranged:
                    OnRangedWeaponChanged?.Invoke(ranged);
                    _onWeaponsChanged.itemType = WeaponType.Ranged;
                    _onWeaponsChanged.weaponSprite = item.ItemIcon;
                    EventsAPI.Invoke(ref _onWeaponsChanged);
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

        public void SetMaxAmmo(int maxAmmo)
        {
            _maxAmmo = maxAmmo;
            OnMaxAmmoChanged?.Invoke(_maxAmmo);
        }

        public void SetCurrentAmmo(int currentAmmo)
        {
            _currentAmmo = currentAmmo;
            OnCurrentAmmoChanged?.Invoke(_currentAmmo);
        }

        public void ModifyCurrentAmmo(int value)
        {
            _currentAmmo += value;
            _currentAmmo = Mathf.Clamp(_currentAmmo, 0, _maxAmmo);
            OnCurrentAmmoChanged?.Invoke(_currentAmmo);
        }

        private void OnRangedAttack(ref OnPlayerRangeAttackPerformedEvent dummy) => ModifyCurrentAmmo(-1);
        

        public void DetachListeners()
        {
            ItemSlotBase.OnItemRemoved -= DropItemOnGround;
            EventsAPI.Unregister<OnPlayerRangeAttackPerformedEvent>(OnRangedAttack);

        }



        public void TearDown()
        {

        }
    }
}