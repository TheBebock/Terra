using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasHealth 
{
    /// <summary>
    /// MaxHealth of the entity 
    /// </summary>
    public float MaxHealth { get; }
    
    /// <summary>
    /// Current health of the entity
    /// </summary>
    public float CurrentHealth { get; }
}
