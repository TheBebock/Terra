using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable : IHasHealth
{
    /// <summary>
    /// Whether entity can be healed
    /// </summary>
    public bool CanBeHealed { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(float amount);
}
