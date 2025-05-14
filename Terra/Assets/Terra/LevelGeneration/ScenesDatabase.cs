using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Terra.LevelGeneration
{
    [CreateAssetMenu(fileName = "SceneList", menuName = "ScriptableObjects/Scene List")]
    public class ScenesDatabase : ScriptableObject
    {
    
        public List<AssetReference> scenes = new ();

    }
}
