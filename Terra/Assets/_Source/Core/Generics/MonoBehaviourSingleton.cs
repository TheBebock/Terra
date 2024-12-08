using System;
using OdinSerializer;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Generics
{
    /// <summary>
    /// Class for MonoBehaviours that should be accessed from other classes
    /// </summary>
    public abstract class MonoBehaviourSingleton<T> : SerializedMonoBehaviour 
        where T : class
    {
        
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
        

        protected virtual void Awake()
        {
           
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}

