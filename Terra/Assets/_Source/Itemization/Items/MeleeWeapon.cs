using System;
using System.Collections.Generic;
using Terra.Combat;
using Terra.Itemization.Items.Definitions;
using Terra.Utils;
using UnityEngine;


namespace Terra.Itemization.Abstracts
{
    [Serializable]
    public class MeleeWeapon : Weapon<MeleeWeaponData>
    {
        public override ItemType ItemType => ItemType.Melee;

        public MeleeWeapon(MeleeWeaponData itemData)
        {
            Data = itemData;
        }
        
        
        public override void PerformAttack(Vector3 position, Quaternion rotation)
        {
            switch (Data.attackType)
            {
                case AttackType.Thrust:
                    PerformThrust(position, rotation);
                    return;
                
                case AttackType.Swing:
                    PerformSwing(position);
                    return;

            }
        }
        private void PerformThrust(Vector3 position, Quaternion rotation)
        {
           List<IDamageable> targets = ComponentProvider.GetTargetsInBox<IDamageable>(position, Data.hitboxSize, ComponentProvider.PlayerTargetsMask,
                rotation);
           if (!CombatManager.Instance)
           {
               Debug.LogError(this + "Combat Manager not found");
               return;
           }
           CombatManager.Instance.PlayerPerformedAttack(targets, Data.damage);
        }

        private void PerformSwing(Vector3 position)
        {
            List<IDamageable> targets = ComponentProvider.GetTargetsInSphere<IDamageable>(position, Data.sphereHitboxRadius, ComponentProvider.PlayerTargetsMask);
            if (!CombatManager.Instance)
            {
                Debug.LogError(this + "Combat Manager not found");
                return;
            }
            CombatManager.Instance.PlayerPerformedAttack(targets, Data.damage);
        }
        
    }
}