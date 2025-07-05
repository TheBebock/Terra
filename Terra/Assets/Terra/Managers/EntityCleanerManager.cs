using System.Collections.Generic;
using Terra.Core.Generics;
using Terra.EventsSystem;
using Terra.EventsSystem.Events;
using Terra.Extensions;
using Terra.Interfaces;
using UnityEngine;

namespace Terra.Managers
{
    public class EntityCleanerManager : PersistentMonoSingleton<EntityCleanerManager>, IAttachListeners
    {
        private List<IRequireCleanup> _registeredEntities = new ();

        public void AttachListeners()
        {
            Debug.Log($"{gameObject.name} test listener attached on entity cleaner manager");
            EventsAPI.Register<PerformCleanupEvent>(OnPerformCleanupEvent);
        }
        
        public void RegisterEntity(IRequireCleanup requireCleanup)
        {
             _registeredEntities.AddUnique(requireCleanup);
        }

        public void UnregisterEntity(IRequireCleanup requireCleanup)
        {
            _registeredEntities.RemoveElement(requireCleanup);
        }

        private void OnPerformCleanupEvent(ref PerformCleanupEvent @event)
        {
            Debug.Log($"{gameObject.name} on clean up performed event");

            for (int i = _registeredEntities.Count - 1; i >= 0; i--)
            {
                _registeredEntities[i]?.PerformCleanup();
                _registeredEntities.RemoveAt(i);
            }
        }

        public void DetachListeners()
        {
            EventsAPI.Unregister<PerformCleanupEvent>(OnPerformCleanupEvent);
        }
    }
}
