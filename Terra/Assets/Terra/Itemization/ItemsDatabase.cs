using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NaughtyAttributes;
using OdinSerializer.Utilities;
using Terra.ID;
using Terra.Itemization.Abstracts.Definitions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Itemization
{

    [CreateAssetMenu(fileName = "ItemsDatabase", menuName = "TheBebocks/Items/Items Database")]
    public class ItemsDatabase : Core.Generics.ScriptableSingleton<ItemsDatabase>
    {
        [FormerlySerializedAs("itemDefinitions")] [SerializeField, ReadOnly] private List<ItemData> _itemDefinitions = new();

        public List<ItemData> ItemDefinitions => _itemDefinitions;

        private static readonly string PassiveItemsPath = Path.Combine("Assets", "_Data", "Items", "Passive");
        private static readonly string ActiveItemsPath = Path.Combine("Assets", "_Data", "Items", "Active");
        private static readonly string MeleeWeaponPath = Path.Combine("Assets", "_Data", "Weapons", "Melee");
        private static readonly string RangedWeaponPath = Path.Combine("Assets", "_Data", "Weapons", "Ranged");


        private void OnEnable()
        {
            for (int i = 0; i < _itemDefinitions.Count; i++)
            {
                IDFactory.RegisterID(_itemDefinitions[i]);
            }
        }

        public void AddItem(string itemName, ItemType itemType)
        {
            if (itemType == ItemType.None || itemName.IsNullOrWhitespace()) return;

            switch (itemType)
            {
                case ItemType.Passive:
                    AddItem(itemName, itemType, PassiveItemsPath);
                    return;

                case ItemType.Active:
                    AddItem(itemName, itemType, ActiveItemsPath);
                    return;

                case ItemType.Melee:
                    AddItem(itemName, itemType, MeleeWeaponPath);
                    return;

                case ItemType.Ranged:
                    AddItem(itemName, itemType, RangedWeaponPath);
                    return;
            }
        }

        public void RemoveItem(string itemName)
        {
            if (string.IsNullOrEmpty(itemName) ||
                !_itemDefinitions.Exists(d => d.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase)))
            {
                Debug.LogError("Remove Item - Error" +
                               $"Couldn't find an item named {itemName}");
                return;
            }

            ItemData item = _itemDefinitions.Find(d => d.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));

            switch (item)
            {
                case PassiveItemData:
                    RemoveItem(itemName, PassiveItemsPath);
                    return;
                case ActiveItemData:
                    RemoveItem(itemName, ActiveItemsPath);
                    return;
                case MeleeWeaponData:
                    RemoveItem(itemName, MeleeWeaponPath);
                    return;
                case RangedWeaponData:
                    RemoveItem(itemName, RangedWeaponPath);
                    return;
            }
        }


        //TODO: Move to generic RuntimeDatabase<T>
        private void AddItem(string itemName, ItemType type, string path)
        {
            string assetPath = string.IsNullOrEmpty(itemName) ? string.Empty : Path.Combine(path, $"{itemName}.asset");
            if (string.IsNullOrEmpty(assetPath) || File.Exists(assetPath) ||
                _itemDefinitions.Any(d => d.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase)))
            {
                Debug.LogError("Add Item - Error" +
                               $"Couldn't create item asset. File already exists or inserted name is invalid.");
                return;
            }

            ItemData itemDefinition = CreateItemDataInstance(type);
            itemDefinition.Initialize(itemName);
            _itemDefinitions.Add(itemDefinition);
#if  UNITY_EDITOR
            AssetDatabase.CreateAsset(itemDefinition, assetPath);
            AssetDatabase.SaveAssets();
#endif
            

        }

        //TODO: Move to generic RuntimeDatabase<T>
        private void RemoveItem(string itemName, string path)
        {
            string assetPath = string.IsNullOrEmpty(itemName) ? string.Empty : Path.Combine(path, $"{itemName}.asset");
            if (string.IsNullOrEmpty(assetPath) ||
                !_itemDefinitions.Exists(d => d.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase)))
            {
                Debug.LogError("Remove Item - Error" +
                               $"Couldn't remove item asset. File name is invalid or item with that name doesn't exist.");
            }
            else
            {
                ItemData itemDefinition = _itemDefinitions.Find(d => d.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                itemDefinition.ReturnID();
                _itemDefinitions.Remove(itemDefinition);
#if  UNITY_EDITOR
                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();
#endif
                
            }
        }

        private ItemData CreateItemDataInstance(ItemType itemType)
        {
            ItemData itemData = null;

            switch (itemType)
            {
                case ItemType.Passive:
                    itemData = ScriptableObject.CreateInstance<PassiveItemData>();
                    break;

                case ItemType.Active:
                    itemData = ScriptableObject.CreateInstance<ActiveItemData>();
                    break;

                case ItemType.Melee:
                    itemData = ScriptableObject.CreateInstance<MeleeWeaponData>();
                    break;

                case ItemType.Ranged:
                    itemData = ScriptableObject.CreateInstance<RangedWeaponData>();
                    break;
            }

            return itemData;
        }
    }
}