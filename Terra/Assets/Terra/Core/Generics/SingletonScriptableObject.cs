using System;
using UnityEngine;

namespace Terra.Core.Generics
{
    public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        // Property to access the singleton instance
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] assets = Resources.LoadAll<T>("");
                    if (assets == null)
                    {
                        throw new Exception("There is no singleton scriptable object of type " + typeof(T).Name);
                    }
                    if (assets.Length > 0)
                    {
                        Debug.LogWarning($"Found multiple instances of {typeof(T).Name}");
                    }
                    
                    _instance = assets[0];
                }
                return _instance;
            }
        }
        
    }
}