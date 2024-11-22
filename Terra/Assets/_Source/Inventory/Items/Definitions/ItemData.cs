using System.Collections;
using System.Collections.Generic;
using Core.ModifiableValue;
using UnityEngine;

namespace Inventory.Items.Definitions
{
    public abstract class ItemData : ScriptableObject
    {
        public List<ValueModifier> stats;
    }

}

