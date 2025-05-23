using System;
using System.Collections.Generic;
using Terra.Combat;
using Terra.Itemization.Abstracts;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Managers;
using Terra.Player;
using Terra.Utils;
using UnityEngine;

namespace Terra.Itemization.Items
{
    [Serializable]
    public class MeleeWeapon : Weapon<MeleeWeaponData>
    {
        private AudioSource audioSource;
        public override ItemType ItemType => ItemType.Melee;

        public MeleeWeapon(MeleeWeaponData itemData)
        {
            Data = itemData;
            audioSource = PlayerManager.Instance.PlayerEntity.GetComponent<AudioSource>();
        }
        
        
        private void PerformThrust(Vector3 position, Quaternion rotation)
        {
           List<IDamageable> targets = ComponentProvider.GetTargetsInBox<IDamageable>(position, 
               Data.hitboxSize, ComponentProvider.PlayerTargetsMask, rotation);
           if (!CombatManager.Instance)
           {
               Debug.LogError(this + "Combat Manager not found");
               return;
           }
           CombatManager.Instance.PerformAttack(PlayerManager.Instance.PlayerEntity, 
               targets, Data.effects, Data.damage);
        }

        private void PerformSwing(Vector3 position)
        {
           
            List<IDamageable> targets = ComponentProvider.GetTargetsInSphere<IDamageable>(position, 
                Data.sphereHitboxRadius, ComponentProvider.PlayerTargetsMask);
            if (!CombatManager.Instance)
            {
                Debug.LogError(this + "Combat Manager not found");
                return;
            }
            CombatManager.Instance.PerformAttack(PlayerManager.Instance.PlayerEntity, 
                targets, Data.effects, Data.damage);
        }
    }
}