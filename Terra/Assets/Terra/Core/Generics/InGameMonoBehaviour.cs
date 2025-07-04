using System;
using System.Threading;
using Terra.ID;
using Terra.Interfaces;
using Terra.Managers;
using UnityEngine;

namespace Terra.Core.Generics
{
    /// <summary>
    /// Class that implements custom initialization pipeline
    /// </summary>
    public abstract class InGameMonobehaviour : MonoBehaviour, IInitializable
    {
        public bool IsInitialized { get; private set; }
        protected CancellationToken CancellationToken => destroyCancellationToken; 
        
        public Action onDestroy;
    
        private void Start()
        {
            if(IsInitialized) return;
        
            IsInitialized = true;
            Initialize();
        
        }

        public void Initialize()
        {
            if(this is IWithSetUp setup)
                setup.SetUp();

            if (this is IAttachListeners attachListeners)
            {
                attachListeners.AttachListeners();
            }
                
            if (this is IUniqueable uniqueable)
            {
                uniqueable.RegisterID();
            }
                
            if (this is IRequireCleanup requireCleanup)
            {
                EntityCleanerManager.Instance?.RegisterEntity(requireCleanup);
            }
        }

        protected virtual void CleanUp()
        { }
        private void OnDestroy()
        {
            onDestroy?.Invoke();
            
            if(this is IAttachListeners attachListeners)
                attachListeners.DetachListeners();
        
            CleanUp();

            if (this is IRequireCleanup requireCleanup)
            {
                EntityCleanerManager.Instance?.UnregisterEntity(requireCleanup);
            }
            
            if(this is IWithSetUp setup)
                setup.TearDown();
        
            if(this is IUniqueable uniqueable)
                uniqueable.ReturnID();
        }

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            
        }
#endif
    }
}
