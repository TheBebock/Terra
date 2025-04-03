using System.Collections;
using System.Collections.Generic;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items.Definitions;
using UnityEngine;

public class TestActiveItem : ActiveItem
{

    public TestActiveItem(ActiveItemData data) : base(data)
    {
        
    }
    public override void OnEquip()
    {
        Debug.Log("TestActiveItem.OnEquip " + Data.cooldown);
    }
}
