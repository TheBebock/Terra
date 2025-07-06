using System.Collections.Generic;
using Terra.Combat;
using Terra.Player;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract;
using Terra.Itemization.Abstracts.Definitions;
using UnityEngine;
using Terra.Utils;
using Terra.Interfaces;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;

namespace Terra.Managers
{

    public class CombatManager : MonoBehaviourSingleton<CombatManager>, IAttachListeners
    {
        [SerializeField][Range(0,100)]private int _basePlayerCritChance = 5;
        [SerializeField, Min(1f)] private float _critDamageMultiplier = 2f;

        private LayerMask _playerLayer;

        private static List<IDamageable> _targetsList = new();
        private static EffectsContainer _allPlayerEffects = new();

        private float _difficultyModifier = 1;

        protected override void Awake()
        {
            base.Awake();
            _playerLayer = LayerMask.NameToLayer("Player");
            SetDifficultyMultiplier();
        }

        public void AttachListeners()
        {
            EventsAPI.Register<GameDifficultyChangedEvent>(OnGameDifficultyChanged);
        }

        private void OnGameDifficultyChanged(ref GameDifficultyChangedEvent gameDifficulty)
        {
            SetDifficultyMultiplier();
        }

        private void SetDifficultyMultiplier()
        {
            switch (GameSettings.DefaultDifficultyLevel)
            {
                case Enums.GameDifficulty.Cyberiada: _difficultyModifier = 2; break;
                case Enums.GameDifficulty.Easy: _difficultyModifier = 1.5f; break;
                case Enums.GameDifficulty.Normal: _difficultyModifier = 1; break;
            }
        }
        
        public void PlayerPerformAttack(WeaponType weaponType, Entity source, IDamageable target,
            EffectsContainer effectsContainer = null, bool isPercentage = false)
        {
            _targetsList.Add(target);
            PlayerPerformAttack(weaponType, source, _targetsList, effectsContainer, isPercentage);
            _targetsList.Clear();
        }
        
        public void PlayerPerformAttack(WeaponType weaponType, Entity source, List<IDamageable> hitTargets,
            EffectsContainer effectsContainer = null, bool isPercentage = false)
        {
            
            if (!PlayerStatsManager.Instance || hitTargets.Count == 0) return;
            
            int statsValue = weaponType == WeaponType.Melee ? 
                PlayerStatsManager.Instance.PlayerStats.Strength
                : PlayerStatsManager.Instance.PlayerStats.Dexterity;
            bool isCrit = CheckForPlayerCrit();

            for (int i = 0; i < hitTargets.Count; i++)
            {
                if (!hitTargets[i].CanBeDamaged) continue;
                
                int finalDamage = Mathf.RoundToInt(statsValue * _difficultyModifier);

                if (isCrit && !isPercentage)
                {
                    finalDamage = Mathf.RoundToInt(finalDamage * _critDamageMultiplier);
                    Debug.Log($"CRIT! Final damage: {finalDamage}");
                }

                ComputePlayerEffects(weaponType, effectsContainer);
                
                hitTargets[i].TakeDamage(finalDamage, isCrit, isPercentage);
                _allPlayerEffects?.ExecuteActions(source, hitTargets[i] as Entity);
                _allPlayerEffects?.ApplyStatuses(hitTargets[i]);
                _allPlayerEffects?.Clear();
            }
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
                PlayerPerformedAttackNotAccountingForWeapon(source, targets, effectsContainer, baseDamage, isPercentage);
            }
            else
            {
                EnemyPerformedAttack(source, targets, baseDamage, effectsContainer, isPercentage);
            }
        }

        private void PlayerPerformedAttackNotAccountingForWeapon(Entity source, List<IDamageable> hitTargets,
            EffectsContainer effectsContainer = null, int baseWeaponDamage = 0, bool isPercentage = false)
        {
            if (!PlayerStatsManager.Instance) return;

            int playerStrengthValue = PlayerStatsManager.Instance.PlayerStats.Strength;
            bool isCrit = CheckForPlayerCrit();

            for (int i = 0; i < hitTargets.Count; i++)
            {
                if (!hitTargets[i].CanBeDamaged) continue;

                
                int finalDamage = Mathf.RoundToInt((baseWeaponDamage + playerStrengthValue) * _difficultyModifier);

                if (isCrit)
                {
                    finalDamage = Mathf.RoundToInt(finalDamage * 2f); // Można przenieść mnożnik do statystyk
                    Debug.Log($"CRIT! Final damage: {finalDamage}");
                }

                EffectsContainer tempEffectsContainer = new EffectsContainer();
                tempEffectsContainer.actions.AddRange(effectsContainer.actions);
                
                tempEffectsContainer.statuses.AddRange(effectsContainer.statuses);
                
                hitTargets[i].TakeDamage(finalDamage, isCrit, isPercentage);
                effectsContainer?.ExecuteActions(source, hitTargets[i] as Entity);
                effectsContainer?.ApplyStatuses(hitTargets[i]);
            }
        }

        private void EnemyPerformedAttack(Entity source, List<IDamageable> hitTargets, int damage,
            EffectsContainer effectsContainer = null, bool isPercentage = false)
        {
            for (int i = 0; i < hitTargets.Count; i++)
            {
                hitTargets[i].TakeDamage(damage, false, isPercentage);

                if (!hitTargets[i].CanBeDamaged) continue;
                effectsContainer?.ExecuteActions(source, hitTargets[i] as Entity);
                effectsContainer?.ApplyStatuses(hitTargets[i]);
            }
        }

        private bool CheckForPlayerCrit()
        {
            
            float luck = PlayerStatsManager.Instance.PlayerStats.Luck;
            float baseCritChance = _basePlayerCritChance;

            float totalCritChance = baseCritChance + luck;
            float roll = Random.Range(0, 100);

            return roll <= totalCritChance;
        }

        private void ComputePlayerEffects(WeaponType type, EffectsContainer defaultWeaponEffects = null)
        {
            _allPlayerEffects.Clear();
            switch (type)
            {
                case WeaponType.Melee:
                    _allPlayerEffects.AddEffects(PlayerManager.Instance.PlayerAttackController.MeleeEffectContainer);
                    break;
                case WeaponType.Ranged:
                    _allPlayerEffects.AddEffects(PlayerManager.Instance.PlayerAttackController.RangeEffectContainer);
                    break;
            }
            _allPlayerEffects.AddEffects(defaultWeaponEffects);
        }

        public void DetachListeners()
        {
            EventsAPI.Unregister<GameDifficultyChangedEvent>(OnGameDifficultyChanged);
        }
    }
}