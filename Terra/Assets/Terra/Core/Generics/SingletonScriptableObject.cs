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
                    // Try to load the instance if it's not found
                    _instance = Resources.FindObjectsOfTypeAll<T>()[0];

                    if (_instance == null)
                    {
                        // If no instance is found, create a new one
                        _instance = CreateInstance<T>();

                        // This is important to avoid the instance being destroyed when the scene changes
                        DontDestroyOnLoad(_instance);
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this as T)
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}