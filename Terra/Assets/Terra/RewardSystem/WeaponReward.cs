﻿using Terra.Itemization.Abstracts.Definitions;
using Terra.Itemization.Items;
using Terra.Managers;
using Terra.Player;
using UnityEngine;

namespace Terra.RewardSystem
{
    public class WeaponReward: RewardData
    {
        public WeaponType WeaponType { get; set; }
        public MeleeWeapon MeleeWeapon { get; set; }
        public RangedWeapon RangedWeapon { get; set; }

        public override void ApplyReward()
        {
            if (WeaponType == WeaponType.Melee)
            {
                Debug.Log($"New Item Swap Status: {PlayerInventoryManager.Instance.TryToEquipItem(MeleeWeapon)}");
                LootManager.Instance.LootTable.RemoveMeleeWeapon(MeleeWeapon);
            }
            else if (WeaponType == WeaponType.Ranged)
            {
                PlayerInventoryManager.Instance.TryToEquipItem(RangedWeapon);
                LootManager.Instance.LootTable.RemoveRangedWeapon(RangedWeapon);
            }
        }
    }
}