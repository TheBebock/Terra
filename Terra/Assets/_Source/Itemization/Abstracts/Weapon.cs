using System;
using Terra.Itemization.Items.Definitions;
using UnityEngine;

namespace Terra.Itemization.Abstracts
{
    /// <summary>
    /// Represents logic for the weapon.
    /// </summary>
    /// <remarks>Class should be an abstract, do not create instances of this class</remarks>
    [Serializable]
    public class Weapon<TData> : Item<TData>
    where TData : WeaponData
    {
        public virtual void PerformAttack(Vector3 position, Quaternion rotation)
        {
            
        }

        protected Weapon()
        {
            return;
        }
        
    }
}