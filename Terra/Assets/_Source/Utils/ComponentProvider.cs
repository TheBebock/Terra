using System.Collections.Generic;
using UnityEngine;

namespace Terra.Utils
{
    public static class ComponentProvider
{
    public static LayerMask PlayerTargetsMask => (1 << LayerMask.NameToLayer("Enemy")) | 
                                                 (1 << LayerMask.NameToLayer("Damageable"));

    public static LayerMask EnemyTargetsMask => (1 << LayerMask.NameToLayer("Player")) |
                                                (1 << LayerMask.NameToLayer("Damageable"));

    public static List<T> GetTargetsInSphere<T>(Vector3 position, float radius, LayerMask targetMask)
    {
        // Initialize collections
        List<T> results = new List<T>(); 
        Collider[] hitColliders = new Collider[32];
        
        Physics.OverlapSphereNonAlloc(position, radius, hitColliders, targetMask);
        
        // Loop through colliders to find target components 
        for (int i = 0; i < hitColliders.Length; i++)
        {
            // Break if null, as all further indexes will also be null
            if(hitColliders[i] == null) break;
            // Get target component
            T target = hitColliders[i].gameObject.GetComponentInChildren<T>();
            
            if (target != null)
            {
                // If found, add to results
                results.Add(target);
                
                Debug.Log($"Found target {target}");
            }
            
        }
        return results;
    }
    
    public static List<T> GetTargetsInBox<T>(Vector3 offset, Vector3 extents, LayerMask targetMask) =>
        GetTargetsInBox<T>(offset, extents, targetMask, Quaternion.identity);
    
    public static List<T> GetTargetsInBox<T>(Vector3 offset, Vector3 extents, LayerMask targetMask, Quaternion rotation)
    {
        // Initialize collections
        List<T> results = new List<T>(); 
        Collider[] hitColliders = new Collider[32];
        
        // Get contacts inside box and cache them to hitColliders
        Physics.OverlapBoxNonAlloc(offset, extents, hitColliders, rotation, targetMask);
        
        // Loop through colliders to find target components 
        for (int i = 0; i < hitColliders.Length; i++)
        {
            // Break if null, as all further indexes will also be null
            if(hitColliders[i] == null) break;
            // Get target component
            T target = hitColliders[i].gameObject.GetComponentInChildren<T>();
            
            if (target != null)
            {
                // If found, add to results
                results.Add(target);
            }
            
        }
        
        return results;
    }
}

}
