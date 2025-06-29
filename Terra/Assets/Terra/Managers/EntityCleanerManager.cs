using System.Collections.Generic;
using NaughtyAttributes;
using Terra.Core.Generics;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Extensions;
using Terra.Interfaces;
using UIExtensionPackage.UISystem.Core.Generics;
using UnityEngine;

namespace Terra.Managers
{
    public class EntityCleanerManager : PersistentSingleton<EntityCleanerManager>, IAttachListeners
    {
        [SerializeField, ReadOnly] private List<Entity> _registeredEntities = new ();

        public void AttachListeners()
        {
            EventsAPI.Register<PerformCleanupEvent>(OnPerformCleanupEvent);
        }
        
        public void RegisterEntity(IRequireCleanup requireCleanup)
        {
            if (requireCleanup is Entity entity)
            {
                _registeredEntities.AddUnique(entity);
            }
            else
            {
                Debug.LogError($"{requireCleanup} is not a Entity implement");
            }
        }

        public void UnregisterEntity(IRequireCleanup requireCleanup)
        {
            if (requireCleanup is Entity entity)
            {
                _registeredEntities.RemoveElement(entity);
            }
            else
            {
                Debug.LogError($"{requireCleanup} is not a Entity implement");
            }
        }

        private void OnPerformCleanupEvent(ref PerformCleanupEvent @event)
        {
            for (int i = _registeredEntities.Count - 1; i >= 0; i--)
            {
                (_registeredEntities[i] as IRequireCleanup)?.PerformCleanup();
                _registeredEntities.RemoveAt(i);
            }
        }

        public void DetachListeners()
        {
            EventsAPI.Unregister<PerformCleanupEvent>(OnPerformCleanupEvent);
        }
    }
}
