using UnityEngine;

namespace Terra.Core.Generics
{
 
    /// <summary>
    /// Class for Singleton MonoBehaviours that aren't supposed to be destroyed on load
    /// </summary>
    public abstract class PersistentMonoSingleton<T> : MonoBehaviourSingleton<T>
        where T : class
    {
        protected override void Awake()
        {
            // In case of a duplicate, it will get destroyed in base.Awake()
            base.Awake();
            if(Instance != this as T) return;
            
            // Ensure that object is on the root level as Unity only allows root level objects to be made persistent.
            if (transform.parent)
            {
                transform.SetParent(null);
                Debug.LogWarning($"{gameObject.name} was not a root object, it has been moved to root level.");
            }
            // Mark this object as DontDestroyOnLoad
            DontDestroyOnLoad(gameObject);
            
        }
    }
}