using System.Collections;
using System.Collections.Generic;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Items.Definitions;
using UnityEngine;

public class TestMeleeWeapon : MeleeWeapon
{
    
    public TestMeleeWeapon(MeleeWeaponData data) : base(data)
    {
        
    }

    public override void PerformAttack(Vector3 position, Quaternion rotation)
    {
        base.PerformAttack(position, rotation);
        
        // Additional logic
    }
}
