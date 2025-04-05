using System;
using OdinSerializer;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Terra.Core.Generics
{
    /// <summary>
    /// Class for MonoBehaviours that should be accessed from other classes
    /// </summary>
    public abstract class MonoBehaviourSingleton<T> : InGameMonobehaviour, IInitializable
        where T : class
    {
        
        public bool IsInitialized { get; set; }
        
        private static T _instance = null;

        public static T Instance
        {
            get
            {

                if(_instance != null) return _instance;
                
                Type type = typeof(T);
                Object[] obj = Object.FindObjectsByType(type, FindObjectsSortMode.None);
                
                if (obj.Length > 0)
                {
                    if (obj.Length > 1)
                    {
                        Debug.LogWarning($"Found duplicate of singleton {type.Name}!" );
                        Destroy(obj[1]);
                    }

                    return obj[0] as T;
                } 
                
                return null;
            }
        }
        
        
        public virtual void Initialize()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this as T)
            {
                Destroy(gameObject);
            }
        }

        protected override void CleanUp()
        {
            if (_instance == this as T)
                _instance = null;
            
            base.CleanUp();
        }
    }
}

