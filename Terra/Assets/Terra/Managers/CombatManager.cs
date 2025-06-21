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
            int baseDamage = 0, bool isPercentage = false ,bool isCrit = false)
        {
            _targetsList.Add(target);
            PerformAttack(source, _targetsList, effectsContainer, baseDamage, isPercentage, isCrit);
            _targetsList.Clear();
        }

        public void PerformAttack(Entity source, List<IDamageable> targets, EffectsContainer effectsContainer = null,
            int baseDamage = 0, bool isPercentage = false, bool isCrit = false)
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
            EffectsContainer effectsContainer = null, int baseWeaponDamage = 0, bool isPercentage = false, bool isCrit = false)
        {
            if (!PlayerStatsManager.Instance) return;
            int playerStrengthValue = PlayerStatsManager.Instance.PlayerStats.Strength;

            for (int i = 0; i < hitTargets.Count; i++)
            {
                int finalDamage = baseWeaponDamage + playerStrengthValue;

                if (isCrit)
                {
                    finalDamage = Mathf.RoundToInt(finalDamage * 2f); // multiplier można potem dać do statystyk
                    Debug.Log($"CRIT! Final damage: {finalDamage}");
                }

                hitTargets[i].TakeDamage(finalDamage, isPercentage);

                if (!hitTargets[i].CanBeDamaged) continue;
                effectsContainer?.ExecuteActions(source, hitTargets[i] as Entity);
                effectsContainer?.ApplyStatuses(hitTargets[i]);
            }
        }


        private void EnemyPerformedAttack(Entity source, List<IDamageable> hitTargets, int damage,
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