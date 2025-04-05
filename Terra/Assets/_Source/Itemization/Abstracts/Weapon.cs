using System;
using Terra.Itemization.Items.Definitions;
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
        public virtual void PerformAttack(Vector3 position, Quaternion rotation)
        {
            
        }
    }
}