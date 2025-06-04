using System;
using Terra.Itemization.Abstracts.Definitions;

namespace Terra.Itemization.Abstracts
{
    /// <summary>
    /// Represents logic for the weapon.
    /// </summary>
    [Serializable]
    public abstract class Weapon<TData> : Item<TData>
    where TData : WeaponData
    {
        
    }
}