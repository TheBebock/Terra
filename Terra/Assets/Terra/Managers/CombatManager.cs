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
        private static List<IDamageable> _targetsList = new();

        protected override void Awake()
        {
            base.Awake();
            _playerLayer = LayerMask.NameToLayer("Player");
        }

        public void PerformAttack(Entity source, IDamageable target, EffectsContainer effectsContainer = null,
            int baseDamage = 0, bool isPercentage = false)
        {
            _targetsList.Add(target);
            PerformAttack(source, _targetsList, effectsContainer, baseDamage, isPercentage);
            _targetsList.Clear();
        }

        public void PerformAttack(Entity source, List<IDamageable> targets, EffectsContainer effectsContainer = null,
            int baseDamage = 0, bool isPercentage = false)
        {
            if (source == null) return;

            if (source.gameObject.layer == _playerLayer)
            {
                PlayerPerformedAttack(source, targets, effectsContainer, baseDamage, isPercentage);
            }
            else
            {
                EnemyPerformedAttack(source, targets, baseDamage, effectsContainer, isPercentage);
            }
        }

        private void PlayerPerformedAttack(Entity source, List<IDamageable> hitTargets,
            EffectsContainer effectsContainer = null, int baseWeaponDamage = 0, bool isPercentage = false)
        {
            if (!PlayerStatsManager.Instance) return;

            int playerStrengthValue = PlayerStatsManager.Instance.PlayerStats.Strength;
            bool isCrit = CheckForPlayerCrit();

            for (int i = 0; i < hitTargets.Count; i++)
            {
                if (!hitTargets[i].CanBeDamaged) continue;

                
                int finalDamage = baseWeaponDamage + playerStrengthValue;

                if (isCrit)
                {
                    finalDamage = Mathf.RoundToInt(finalDamage * 2f); // Można przenieść mnożnik do statystyk
                    Debug.Log($"CRIT! Final damage: {finalDamage}");
                }


                hitTargets[i].TakeDamage(finalDamage, isPercentage);
                effectsContainer?.ExecuteActions(source, hitTargets[i] as Entity);
                effectsContainer?.ApplyStatuses(hitTargets[i]);
            }
        }

        private void EnemyPerformedAttack(Entity source, List<IDamageable> hitTargets, int damage,
            EffectsContainer effectsContainer = null, bool isPercentage = false)
        {
            for (int i = 0; i < hitTargets.Count; i++)
            {
                hitTargets[i].TakeDamage(damage, isPercentage);

                if (!hitTargets[i].CanBeDamaged) continue;
                effectsContainer?.ExecuteActions(source, hitTargets[i] as Entity);
                effectsContainer?.ApplyStatuses(hitTargets[i]);
            }
        }

        private bool CheckForPlayerCrit()
        {
            // Prosty przykład, np. 10% szansy + bonus z broni (później można to rozbudować)
            int luck = PlayerStatsManager.Instance.PlayerStats.Luck;
            float baseCritChance = 0.1f; // 10%
            float luckBonus = luck * 0.01f; // np. 1% za punkt szczęścia

            float totalCritChance = baseCritChance + luckBonus;
            float roll = Random.Range(0, 100);

            return roll < totalCritChance;
        }
    }
}