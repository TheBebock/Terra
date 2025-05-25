using System.Collections.Generic;
using Terra.Combat;
using Terra.Player;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract;
using UnityEngine;

namespace Terra.Managers
{

    public class CombatManager : MonoBehaviourSingleton<CombatManager>
    {

        private LayerMask _playerLayer;

        // Lists exists to avoid multiple instances of new lists when there is only single target being passed
        private static List<IDamageable> _targetsList = new ();

        protected override void Awake()
        {
            base.Awake();
            _playerLayer = LayerMask.NameToLayer("Player");
        }

        public void PerformAttack(Entity source, IDamageable target, EffectsContainer effectsContainer = null,
            float baseDamage = 0, bool isPercentage = false)
        {
            _targetsList.Add(target);
            PerformAttack(source, _targetsList, effectsContainer, baseDamage, isPercentage);
            _targetsList.Clear();
        }

        public void PerformAttack(Entity source, List<IDamageable> targets, EffectsContainer effectsContainer = null,
            float baseDamage = 0, bool isPercentage = false)
        {
            if (source == null) return;

            LayerMask sourceMask = source.gameObject.layer;
            if (sourceMask == _playerLayer)
            {
                PlayerPerformedAttack(source, targets, effectsContainer, baseDamage, isPercentage);
            }
            else
            {
                EnemyPerformedAttack(source, targets, baseDamage, effectsContainer, isPercentage);
            }
        }

        private void PlayerPerformedAttack(Entity source, List<IDamageable> hitTargets,
            EffectsContainer effectsContainer = null, float baseWeaponDamage = 0, bool isPercentage = false)
        {
            if (!PlayerStatsManager.Instance) return;
            // Get damage modifier from player stats
            float playerStrengthValue = PlayerStatsManager.Instance.PlayerStats.Strength;
            // Compute final damage value
            float finalDamage = baseWeaponDamage + playerStrengthValue;

            // Loop through targets and apply damage
            for (int i = 0; i < hitTargets.Count; i++)
            {
                // This will display Immune in case the target is not damageable
                hitTargets[i].TakeDamage(finalDamage, isPercentage);

                if (!hitTargets[i].CanBeDamaged) continue;
                effectsContainer?.ExecuteActions(source, hitTargets[i] as Entity);
                effectsContainer?.ApplyStatuses(hitTargets[i]);
            }
        }

        private void EnemyPerformedAttack(Entity source, List<IDamageable> hitTargets, float damage,
            EffectsContainer effectsContainer = null, bool isPercentage = false)
        {
            // Loop through targets and apply damage
            for (int i = 0; i < hitTargets.Count; i++)
            {
                // This will display Immune in case the target is not damageable
                hitTargets[i].TakeDamage(damage, isPercentage);

                if (!hitTargets[i].CanBeDamaged) continue;
                effectsContainer?.ExecuteActions(source, hitTargets[i] as Entity);
                effectsContainer?.ApplyStatuses(hitTargets[i]);
            }
        }
    }
}