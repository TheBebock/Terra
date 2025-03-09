using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inventory.Items.Definitions;
using NaughtyAttributes;
using OdinSerializer.Utilities;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "TheBebocks/Items/Items Database")]
public class ItemsDatabase : ScriptableObject
{
    [SerializeField, ReadOnly] private List<ItemData> itemDefinitions;

    public List<ItemData> ItemDefinitions => itemDefinitions;
    
    private static readonly string passiveItemsPath = Path.Combine("Assets", "Data", "Items", "Passive");
    private static readonly string activeItemsPath = Path.Combine("Assets", "Data", "Items", "Active");
    private static readonly string meleeWeaponPath = Path.Combine("Assets", "Data", "Weapons", "Melee");
    private static readonly string rangedWeaponPath = Path.Combine("Assets", "Data", "Weapons", "Ranged");


    private void OnEnable()
    {
        for (int i = 0; i < itemDefinitions.Count; i++)
        {
            IDFactory.RegisterID(itemDefinitions[i]);
        }
    }

    public void AddItem(string itemName, ItemType itemType)
    {
        if(itemType == ItemType.None || itemName.IsNullOrWhitespace()) return;
        
        switch (itemType)
        {
            case ItemType.Passive:
                AddItem(itemName, passiveItemsPath);
                return;
            
            case ItemType.Active:
                AddItem(itemName, activeItemsPath);
                return;
            
            case ItemType.Melee:
                AddItem(itemName, meleeWeaponPath);
                return;
            
            case ItemType.Ranged:
                AddItem(itemName, rangedWeaponPath);
                return;
        }
    }
    
    public void RemoveItem(string itemName)
    {
        if (string.IsNullOrEmpty(itemName) || !itemDefinitions.Exists(d => d.itemName == itemName))
        {
            Debug.LogError("Add Item - Error" +
                           $"Couldn't create item asset. File already exists or inserted name is invalid.");
            return;
        }
        
        ItemData item = itemDefinitions.Find(d => d.itemName == itemName);

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
    private void AddItem(string itemName, string path)
    {
        string assetPath = string.IsNullOrEmpty(itemName) ? string.Empty : Path.Combine(path, $"{itemName}.asset");
        if (string.IsNullOrEmpty(assetPath) || File.Exists(assetPath) ||
            itemDefinitions.Any(d => d.itemName == itemName))
        {
            Debug.LogError("Add Item - Error" +
                           $"Couldn't create item asset. File already exists or inserted name is invalid.");
            return;
        }

        ItemData itemDefinition = ScriptableObject.CreateInstance<ItemData>();
        itemDefinition.Initialize(itemName);
        itemDefinitions.Add(itemDefinition);
        AssetDatabase.CreateAsset(itemDefinition, assetPath);
        AssetDatabase.SaveAssets();

    }

    private void RemoveItem(string itemName, string path)
    {
        string assetPath = string.IsNullOrEmpty(itemName) ? string.Empty : Path.Combine(path, $"{itemName}.asset");
        if (string.IsNullOrEmpty(assetPath)  || !itemDefinitions.Exists(d => d.itemName.ToLower() == itemName.ToLower()))
        {
            Debug.LogError("Remove Item - Error" + $"Couldn't remove item asset. File name is invalid or item with that name doesn't exist.");
        }
        else
        {
            ItemData itemDefinition = itemDefinitions.Find(d => d.itemName.ToLower() == itemName.ToLower());
            itemDefinition.ReturnID();
            itemDefinitions.Remove(itemDefinition);
            
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.SaveAssets();
            
          
        }
    }
}
