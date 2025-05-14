using UnityEditor;
using UnityEngine;
using Terra.EffectsSystem.Actions;

namespace Terra.EffectsSystem.Utils
{
    /// <summary>
    ///     Responsible for filling <see cref="StatusEffectMapping"/> with data
    /// </summary>
    public static class ActionEffectsDatabaseGenerator
    {
        [MenuItem("Tools/Generate Action Effects Database")]
        public static void GenerateActionEffectsDatabase()
        {
            const string path = "Assets/Resources/ActionEffectsDatabase.asset";

            ActionEffectsDatabase asset = ScriptableObject.CreateInstance<ActionEffectsDatabase>();
            
            asset.GenerateActionsDatabase();

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

        }
    }
}