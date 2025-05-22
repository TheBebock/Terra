using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.ModifiableValue;
using Terra.ID;
using UnityEngine;

namespace Terra.Itemization.Abstracts.Definitions
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
    public abstract class ItemData : ScriptableObject, IUniqueable 
    {
        [ReadOnly, SerializeField] private int id = -1;
        [ReadOnly] public string itemName;
        public string itemDescription;
        public Sprite itemSprite;
        public bool canBeRemoved = true;
        
        public List<ValueModifier> strengthModifiers;
        public List<ValueModifier> maxHealthModifiers;
        public List<ValueModifier> speedModifiers;
        public List<ValueModifier> luckModifiers;
        
        public int Identity => id;


        public void Initialize(string itemName)
        {
            this.itemName = itemName;
            RegisterID();
        }
        
        public void RegisterID()
        {
            IDFactory.RegisterID(this);
        }

        public void ReturnID()
        {
            IDFactory.ReturnID(this);
        }

        public void SetID(int newID)
        {
            id = newID;
        }

        private void OnValidate()
        {
            // Set modifier IDs
            UpdateModifierIDs(strengthModifiers);
            UpdateModifierIDs(maxHealthModifiers);
            UpdateModifierIDs(speedModifiers);
            UpdateModifierIDs(luckModifiers);
            
            // test 
            //int test = IDFactory.GetNewUniqueId();
            
            // Ensure the ID is set and persistent
            if(Identity != Utils.Constants.DEFAULT_ID) return;
            RegisterID();
            
        }

        private void UpdateModifierIDs(List<ValueModifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                if(modifiers[i].SourceID == Utils.Constants.DEFAULT_ID) continue;

                modifiers[i].SourceID = Identity;
            }
        }
    }
}

