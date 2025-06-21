using System.Collections.Generic;
using JetBrains.Annotations;

namespace Terra.EventsSystem
{
    public interface IEventHandler
    {
        public void Clear();
    }

    internal sealed class EventHandler<TEvent> : IEventHandler 
        where TEvent : struct, IEvent 
    {
        
        private static EventHandler<TEvent> _instance;

        private static EventHandler<TEvent> GetInstance() => _instance ??= new EventHandler<TEvent>();

        /// <summary>
        /// List of all cached requests
        /// </summary>
        private readonly List<CallbackDelegate<TEvent>> _registeredCallbacks = new();
        
        /// <summary>
        /// Registers request callback
        /// </summary>
        public static void RegisterCallback(CallbackDelegate<TEvent> callback)
        {
            EventHandler<TEvent> instance = GetInstance();
            
            EventsAPI.EnsureHandlerStored(instance);
            

            // Register callback to instance if it does not exist
            if (!instance._registeredCallbacks.Contains(callback))
            {
                instance._registeredCallbacks.Add(callback);
            }
        }

        /// <summary>
        /// Unregister callback from this handler
        /// </summary>
        public static void UnregisterCallback([NotNull] CallbackDelegate<TEvent> callback)
        {
            EventHandler<TEvent> instance = GetInstance();
            
            // Unregister callback
            if (instance._registeredCallbacks.Contains(callback))
            {
                instance._registeredCallbacks.Remove(callback);
            }

            if (instance._registeredCallbacks.Count <= 0)
            {
                EventsAPI.RemoveEmptyHandler(instance);
            }
        }

        public static void Invoke(ref TEvent data)
        {
            var callbacks = GetInstance()._registeredCallbacks;
            foreach (CallbackDelegate<TEvent> callbackDelegate in callbacks)
            {
                callbackDelegate.Invoke(ref data);
            }
        }
        
        public void Clear()
        {
            _registeredCallbacks.Clear();
        }
    }
}