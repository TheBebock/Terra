using System.Collections;
using System.Collections.Generic;
using Terra.Combat;
using Terra.Player;
using Terra.Core.Generics;
using Terra.EffectsSystem;
using UnityEngine;

public class CombatManager : MonoBehaviourSingleton<CombatManager>
{
    public void PlayerPerformedAttack(Entity source, List<IDamageable> hitTargets, 
        EffectsContainer effectsContainer = null, float baseWeaponDamage = 0, bool isPercentage = false)
    {
        if(!PlayerStatsManager.Instance) return;
        // Get damage modifier from player stats
        float playerStrengthValue = PlayerStatsManager.Instance.PlayerStats.Strength;
        // Compute final damage value
        float finalDamage = baseWeaponDamage + playerStrengthValue;
        
        // Loop through targets and apply damage
        for (int i = 0; i < hitTargets.Count; i++)
        {
            // This will display Immune in case the target is not damageable
            hitTargets[i].TakeDamage(finalDamage, isPercentage);
            
            if(!hitTargets[i].CanBeDamaged) continue;
            effectsContainer?.ExecuteActions(source, hitTargets[i] as Entity );
            effectsContainer?.ApplyStatuses(hitTargets[i]);
        }
    }
    
    public void EnemyPerformedAttack(Entity source, List<IDamageable> hitTargets, float damage, 
        EffectsContainer effectsContainer = null, bool isPercentage = false)
    {
        // Loop through targets and apply damage
        for (int i = 0; i < hitTargets.Count; i++)
        {
            // This will display Immune in case the target is not damageable
            hitTargets[i].TakeDamage(damage, isPercentage);
            
            if(!hitTargets[i].CanBeDamaged) continue;
            effectsContainer?.ExecuteActions(source, hitTargets[i] as Entity );
            effectsContainer?.ApplyStatuses(hitTargets[i]);
        }
    }
}
