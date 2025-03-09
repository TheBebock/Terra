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
        None = -1,
        Passive = 0,
        Active = 1,
        Melee = 2,
        Ranged = 3,
    }
    [Serializable]
    public class ItemData : ScriptableObject, IUniquable 
    {
        [ReadOnly, SerializeField] private int id = -1;
        public string itemName;
        public string itemDescription;
        public Sprite itemSprite;
        public bool canBeRemoved;
        public List<ValueModifier> strengthModifiers;
        public List<ValueModifier> maxHealthModifiers;
        public List<ValueModifier> speedModifiers;
        public List<ValueModifier> luckModifiers;
        public int Identity => id;


        public void Initialize(string itemName)
        {
            this.itemName = itemName;
            id = IDFactory.GetNewUniqueId();
        }
        
        public void RegisterID()
        {
            //NOTE: No need to register
        }

        public void ReturnID()
        {
            // No real way to return
            IDFactory.ReturnID(this);
        }

        private void OnValidate()
        {
        
            // Ensure the ID is set and persistent
            int test = IDFactory.GetNewUniqueId(true);
            if(Identity != -1) return;
            id = IDFactory.GetNewUniqueId();

        }
    }
}

