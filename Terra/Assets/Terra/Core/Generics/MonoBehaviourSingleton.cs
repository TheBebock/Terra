using System;
using NaughtyAttributes;
using Terra.ID;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Terra.Core.Generics
{
    /// <summary>
    /// Class for MonoBehaviours that should be accessed from other classes
    /// </summary>
    public abstract class MonoBehaviourSingleton<T> : InGameMonobehaviour, IUniqueable
        where T : class
    {
        
        [Foldout("Debug"), ReadOnly] 
        [SerializeField] private int _id = Utils.Constants.DefaultID;
        public int Identity => _id;
        
        private static T _instance;

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
        
        public void RegisterID()
        {
            IDFactory.RegisterID(this);
        }

        public void ReturnID()
        {
            IDFactory.ReturnID(this);
        }

        public void SetID(int newID)
        {
            _id = newID;
        }
    }
}

