using System;
using System.Collections.Generic;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstracts;
using UnityEngine;

namespace Terra.EffectsSystem.Utils
{
    /// <summary>
    ///     Factory for creating <see cref="StatusEffectBase"/>
    /// </summary>
    public static class StatusEffectsFactory
    {
        private static readonly Dictionary<Type, Type> _dataToStatusMap = new Dictionary<Type, Type>();

        static StatusEffectsFactory()
        {
            InitializeFactory();
        }

        private static void InitializeFactory()
        {
            if (!_dataToStatusMap.IsNullOrEmpty()) return;

            var mappingAsset = Resources.Load<StatusEffectMapping>(nameof(StatusEffectMapping));
            if (mappingAsset == null)
            {
                Debug.LogError(
                    "StatusEffectMapping asset not found! Please generate it from the Tools menu on top of the screen.");
                return;
            }

            foreach (var mapping in mappingAsset.mappings)
            {
                var dataType = Type.GetType(mapping.dataTypeName);
                var effectType = Type.GetType(mapping.effectTypeName);

                if (dataType == null || effectType == null)
                {
                    Debug.LogError($"Failed to resolve types for {mapping.dataTypeName} -> {mapping.effectTypeName}");
                    continue;
                }

                _dataToStatusMap[dataType] = effectType;
            }

            Debug.Log($"{nameof(StatusEffectsFactory)} initialized with {_dataToStatusMap.Count} effects.");
        }

        public static StatusEffectBase CreateStatusEffect(Entity target, StatusEffectData data)
        {
            var dataType = data.GetType();

            if (!_dataToStatusMap.TryGetValue(dataType, out var statusType) || statusType == null)
            {
                Debug.LogError($"No StatusEffect mapped for data type {dataType.Name}");
                return null;
            }

            if (Activator.CreateInstance(statusType) is not StatusEffectBase effect)
            {
                Debug.LogError($"Type {statusType.FullName} could not be instantiated as StatusEffectBase.");
                return null;
            }
            
            effect.Initialize(target, data);
            return effect;

        }
    }
}