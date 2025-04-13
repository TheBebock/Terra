using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NaughtyAttributes;
using OdinSerializer.Utilities;
using Terra.ID;
using Terra.Itemization.Items.Definitions;
using UnityEditor;
using UnityEngine;

namespace Terra.Itemization
{

    [CreateAssetMenu(fileName = "ItemsDatabase", menuName = "TheBebocks/Items/Items Database")]
    public class ItemsDatabase : ScriptableSingleton<ItemsDatabase>
    {
        [SerializeField, ReadOnly] private List<ItemData> itemDefinitions = new();

        public List<ItemData> ItemDefinitions => itemDefinitions; 

        private static readonly string passiveItemsPath = Path.Combine("Assets", "_Data", "Items", "Passive");
        private static readonly string activeItemsPath = Path.Combine("Assets", "_Data", "Items", "Active");
        private static readonly string meleeWeaponPath = Path.Combine("Assets", "_Data", "Weapons", "Melee");
        private static readonly string rangedWeaponPath = Path.Combine("Assets", "_Data", "Weapons", "Ranged");


        private void OnEnable()
        {
            for (int i = 0; i < itemDefinitions.Count; i++)
            {
                IDFactory.RegisterID(itemDefinitions[i]);
            }
        }

        public void AddItem(string itemName, ItemType itemType)
        {
            if (itemType == ItemType.None || itemName.IsNullOrWhitespace()) return;

            switch (itemType)
            {
                case ItemType.Passive:
                    AddItem(itemName, itemType, passiveItemsPath);
                    return;

                case ItemType.Active:
                    AddItem(itemName, itemType, activeItemsPath);
                    return;

                case ItemType.Melee:
                    AddItem(itemName, itemType, meleeWeaponPath);
                    return;

                case ItemType.Ranged:
                    AddItem(itemName, itemType, rangedWeaponPath);
                    return;
            }
        }

        public void RemoveItem(string itemName)
        {
            if (string.IsNullOrEmpty(itemName) ||
                !itemDefinitions.Exists(d => d.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase)))
            {
                Debug.LogError("Remove Item - Error" +
                               $"Couldn't find an item named {itemName}");
                return;
            }

            ItemData item = itemDefinitions.Find(d => d.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));

            switch (item)
            {
                case PassiveItemData:
                    RemoveItem(itemName, passiveItemsPath);
                    return;
                case ActiveItemData:
                    RemoveItem(itemName, activeItemsPath);
                    return;
                case MeleeWeaponData:
                    RemoveItem(itemName, meleeWeaponPath);
                    return;
                case RangedWeaponData:
                    RemoveItem(itemName, rangedWeaponPath);
                    return;
            }
        }


        //TODO: Move to generic RuntimeDatabase<T>
        private void AddItem(string itemName, ItemType type, string path)
        {
            string assetPath = string.IsNullOrEmpty(itemName) ? string.Empty : Path.Combine(path, $"{itemName}.asset");
            if (string.IsNullOrEmpty(assetPath) || File.Exists(assetPath) ||
                itemDefinitions.Any(d => d.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase)))
            {
                Debug.LogError("Add Item - Error" +
                               $"Couldn't create item asset. File already exists or inserted name is invalid.");
                return;
            }

            ItemData itemDefinition = CreateItemDataInstance(type);
            itemDefinition.Initialize(itemName);
            itemDefinitions.Add(itemDefinition);
            AssetDatabase.CreateAsset(itemDefinition, assetPath);
            AssetDatabase.SaveAssets();

        }

        //TODO: Move to generic RuntimeDatabase<T>
        private void RemoveItem(string itemName, string path)
        {
            string assetPath = string.IsNullOrEmpty(itemName) ? string.Empty : Path.Combine(path, $"{itemName}.asset");
            if (string.IsNullOrEmpty(assetPath) ||
                !itemDefinitions.Exists(d => d.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase)))
            {
                Debug.LogError("Remove Item - Error" +
                               $"Couldn't remove item asset. File name is invalid or item with that name doesn't exist.");
            }
            else
            {
                ItemData itemDefinition = itemDefinitions.Find(d => d.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                itemDefinition.ReturnID();
                itemDefinitions.Remove(itemDefinition);

                AssetDatabase.DeleteAsset(assetPath);
                AssetDatabase.SaveAssets();
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