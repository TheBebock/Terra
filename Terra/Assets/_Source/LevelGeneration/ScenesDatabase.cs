using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "SceneList", menuName = "ScriptableObjects/Scene List")]
public class ScenesDatabase : ScriptableObject
{
    
    public List<AssetReference> scenes = new ();

}
