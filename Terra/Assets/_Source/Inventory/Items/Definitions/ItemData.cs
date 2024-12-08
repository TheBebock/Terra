using System;
using System.Collections.Generic;
using Core.ModifiableValue;
using NaughtyAttributes;
using UnityEngine;

namespace Inventory.Items.Definitions
{
    //NOTE: When adding more items, always add at the end, because Unity has problems when enums change their values
    //For example, when 'active = 0' changes to 'active = 1', there are issues with created SOs
    public enum ItemType
    {
        Active = 0,
        Melee = 1,
        Ranged = 2,
        Passive = 3,
    }
    [Serializable]
    public abstract class ItemData : ScriptableObject
    {
        //TODO: Implement UniqueIDFactory
        [ReadOnly] public int ID;
        public string itemName;
        public string itemDescription;
        public Sprite itemSprite;
        public bool canBeRemoved;
        public List<ValueModifier> strengthModifiers;
        public List<ValueModifier> maxHealthModifiers;
        public List<ValueModifier> speedModifiers;
        public List<ValueModifier> luckModifiers;
    }
}

