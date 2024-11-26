using System;
using Inventory.Items.Definitions;
using OdinSerializer;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Abstracts
{
    [Serializable]
    public abstract class Item : ScriptableObject, IEquipable
    {
        public bool CanBeRemoved => data.canBeRemoved; 
        
        public ItemType itemType;
        public ItemData data { get; private set; }
        public void Equip() => Equip(this);
        public void UnEquip() => UnEquip(this);

        protected virtual void Equip(Item item)
        {
            
        }
        
        protected virtual void UnEquip(Item item)
        {
            
        }
    }
}