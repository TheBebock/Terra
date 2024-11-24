using System.Collections.Generic;
using Core.ModifiableValue;
using UnityEngine;

namespace Inventory.Items.Definitions
{
    public abstract class ItemData : ScriptableObject
    {
        public List<ValueModifier> stats;
        public int ID;
        public string itemName;
        public string itemDescription;
        public Sprite icon;
        public string itemType;
    }
}

