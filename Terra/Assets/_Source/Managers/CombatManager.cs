using System.Collections;
using System.Collections.Generic;
using Terra.Player;
using Terra.Core.Generics;
using UnityEngine;

public class CombatManager : MonoBehaviourSingleton<CombatManager>
{
    public void PlayerPerformedAttack(List<IDamagable> hitTargets, float baseWeaponDamage = 0)
    {
        if(!PlayerStatsManager.Instance) return;
        // Get damage modifier from player stats
        float playerStrengthValue = PlayerStatsManager.Instance.PlayerStats.Strength;
        // Compute final damage value
        float finalDamage = baseWeaponDamage + playerStrengthValue;
        
        // Loop through targets and apply damage
        for (int i = 0; i < hitTargets.Count; i++)
        {
            if(!hitTargets[i].CanBeDamaged) continue;
            
            hitTargets[i].TakeDamage(finalDamage);
        }
    }
    
    public void EnemyPerformedAttack(List<IDamagable> hitTargets, float damage)
    {
        // Loop through targets and apply damage
        for (int i = 0; i < hitTargets.Count; i++)
        {
            hitTargets[i].TakeDamage(damage);
        }
    }
}
