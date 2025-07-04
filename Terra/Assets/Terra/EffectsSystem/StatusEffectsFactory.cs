using System;
using System.Collections.Generic;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Abstract.Definitions;
using Terra.Extensions;
using UnityEngine;

namespace Terra.EffectsSystem
{
    /// <summary>
    ///     Factory for creating <see cref="StatusEffectBase"/>
    /// </summary>
    public static class StatusEffectsFactory
    {
        private static readonly Dictionary<Type, Type> DataToStatusMap = new Dictionary<Type, Type>();

        static StatusEffectsFactory()
        {
            InitializeFactory();
        }

        private static void InitializeFactory()
        {
            if (!DataToStatusMap.IsNullOrEmpty()) return;

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

                DataToStatusMap[dataType] = effectType;
            }

            Debug.Log($"{nameof(StatusEffectsFactory)} initialized with {DataToStatusMap.Count} effects.");
        }

        public static StatusEffectBase CreateStatusEffect(Entity target, StatusEffectData data)
        {
            var dataType = data.GetType();

            if (!DataToStatusMap.TryGetValue(dataType, out var statusType) || statusType == null)
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