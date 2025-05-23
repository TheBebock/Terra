using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NaughtyAttributes;
using OdinSerializer.Utilities;
using Terra.Attributes;
using Terra.Core.Generics;
using Terra.EffectsSystem.Abstract;
using Terra.EffectsSystem.Abstract.Definitions;
using Terra.Extensions;
using UnityEngine;

namespace Terra.EffectsSystem.Actions
{
    public class ActionEffectsDatabase : ScriptableSingleton<ActionEffectsDatabase>
    {

        [SerializeField, ReadOnly] private List<EffectsMapping> _mapping = new();
        private Dictionary<Type, Type> _dataToActionTypeMap = new();
        private Dictionary<Type, ActionEffectBase> _dataToActionEffectMap = new();
        

        private void OnEnable()
        {
            //NOTE: Unity does not serialize dictionaries, they need to be re-build at runtime, hence the OnEnable initialization
            InitializeDatabase();
        }
        public void GenerateActionsDatabase()
        {
            _mapping.Clear();
        
            var actionEffectTypes = Assembly.GetAssembly(typeof(ActionEffectBase))
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract)
                .Where(type => type.IsSubclassOf(typeof(ActionEffectBase)))
                .Where(type => type.GetCustomAttribute<ActionEffectAttribute>() != null)
                .ToList();
            
            if (!actionEffectTypes.Any())
            {
                Debug.LogError("Action effects not found");
                return;
            }
            foreach (var actionType in actionEffectTypes)
            {
                var attribute = actionType.GetCustomAttribute<ActionEffectAttribute>();
                if (attribute is null)
                {
                    Debug.LogError($"Type {actionType.Name} has no ActionEffectAttribute!");
                    continue;
                }
                _mapping.Add(new EffectsMapping()
                {
                    dataTypeName = attribute.DataType.AssemblyQualifiedName,
                    effectTypeName = actionType.AssemblyQualifiedName
                });
            }
            
            InitializeDatabase();
        }
        private void InitializeDatabase()
        {
            if (!_dataToActionTypeMap.IsNullOrEmpty()) return;
        
            _dataToActionTypeMap.Clear();
            _dataToActionEffectMap.Clear();

            foreach (var mapping in _mapping)
            {
                var dataType = Type.GetType(mapping.dataTypeName);
                var effectType = Type.GetType(mapping.effectTypeName);

                if (dataType == null || effectType == null)
                {
                    Debug.LogError($"Failed to resolve types for {mapping.dataTypeName} -> {mapping.effectTypeName}");
                    continue;
                }

                _dataToActionTypeMap[dataType] = effectType;
            }

            foreach (var effectType in _dataToActionTypeMap)
            {
                var action = Activator.CreateInstance(effectType.Value) as ActionEffectBase;
            
                _dataToActionEffectMap.Add(effectType.Key, action);
            }
            
            Debug.Log($"Actions Database initialized with {_dataToActionEffectMap.Count} effects.");
            
        }

        public void ExecuteAction(ActionEffectData data, Entity target, Entity source = null)
        {
            var dataType = data.GetType();
            if (_dataToActionEffectMap.IsNullOrEmpty())
            {
                Debug.LogError($"{_dataToActionEffectMap} is empty!");
                return;
            }
            if (!_dataToActionEffectMap.TryGetValue(dataType, out var effect) || effect == null)
            {
                Debug.LogError($"No ActionEffect mapped for data type {dataType.Name}");
                return;
            }
            effect.Initialize(target, data);
            effect.Execute(target, source);
        }
    }
}
