using System.Collections;
using System.Collections.Generic;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items.Definitions;
using UnityEngine;

public class TestPassiveItem : PassiveItem
{

    TestPassiveItem(PassiveItemData data) : base(data)
    {
        
    }


    public override void UpdateItem()
    {
        base.UpdateItem();
        // ADDITIONAL LOGIC
    }
}
