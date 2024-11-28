using System;
using System.Collections.Generic;
using Core.ModifiableValue;
using NaughtyAttributes;
using UnityEngine;

namespace Inventory.Items.Definitions
{
    public enum ItemType
    {
        Passive = 0,
        Active = 1,
        Melee = 2,
        Ranged = 3
    }
    [Serializable]
    public abstract class ItemData : ScriptableObject
    {
        //TODO: Implement UniqueIDFactory
        [ReadOnly] public int ID;
        public string itemName;
        public string itemDescription;
        public Sprite icon;
        public bool canBeRemoved;
        public List<ValueModifier> strengthModifiers;
        public List<ValueModifier> maxHealthModifiers;
        public List<ValueModifier> speedModifiers;
        public List<ValueModifier> luckModifiers;
    }
}

