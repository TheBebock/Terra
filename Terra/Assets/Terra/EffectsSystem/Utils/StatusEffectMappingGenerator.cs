using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Reflection;
using Terra.EffectsSystem.Abstract;

namespace Terra.EffectsSystem.Utils
{
    /// <summary>
    ///     Responsible for filling <see cref="StatusEffectMapping"/> with data
    /// </summary>
    public static class StatusEffectMappingGenerator
    {
        [MenuItem("Tools/Generate Status Effect Mapping")]
        public static void GenerateMapping()
        {
            const string path = "Assets/Resources/StatusEffectMapping.asset";

            var asset = ScriptableObject.CreateInstance<StatusEffectMapping>();

            var statusEffectTypes = Assembly.GetAssembly(typeof(StatusEffectBase))
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract)
                .Where(type => type.IsSubclassOf(typeof(StatusEffectBase)))
                .Where(type => type.GetCustomAttribute<StatusEffectAttribute>() != null);

            foreach (var statusType in statusEffectTypes)
            {
                var attribute = statusType.GetCustomAttribute<StatusEffectAttribute>();
                if (attribute == null)
                {
                    Debug.LogWarning($"Type {statusType.Name} has no StatusEffectAttribute!");
                    continue;
                }

                asset.mappings.Add(new StatusEffectMapping.Mapping
                {
                    dataTypeName = attribute.DataType.AssemblyQualifiedName,
                    effectTypeName = statusType.AssemblyQualifiedName
                });
            }

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            Debug.Log($"Generated StatusEffectMapping with {asset.mappings.Count} entries at {path}");
        }
    }
}