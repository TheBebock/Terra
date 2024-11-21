using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable<T> 
where T : Item
{
    public void Equip(T item);
}
