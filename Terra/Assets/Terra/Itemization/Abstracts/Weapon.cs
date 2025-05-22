using System;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Player;
using UnityEngine;

namespace Terra.Itemization.Abstracts
{
    /// <summary>
    /// Represents logic for the weapon.
    /// </summary>
    [Serializable]
    public abstract class Weapon<TData> : Item<TData>
    where TData : WeaponData
    {
        public sealed override void OnEquip()
        {
            base.OnEquip();
            if (!PlayerInventoryManager.Instance)
            {
                Debug.LogError($"{this}: Player Inventory Manager not found, cannot instantiate weapon hitboxes");
                return;
            }
        }
    }
}