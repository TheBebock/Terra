using System;
using OdinSerializer;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Generics
{
 
    /// <summary>
    /// Class for MonoBehaviours that aren't supposed to be destroyed on load
    /// </summary>
    public abstract class GlobalMonoBehaviour<T> : SerializedMonoBehaviour 
        where T : class
    {
        
        private static T _instance = null;

        public static T Instance => _instance;

        protected virtual void Awake()
        {
           
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(this);
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