using System.Collections.Generic;
using UnityEngine;

namespace Terra.EventsSystem
{
    public static class EventsAPI
    {
        
        private static readonly HashSet<IEventHandler> Handlers = new();


        public static void EnsureHandlerStored<TCallbackHandler>(TCallbackHandler handler)
        where TCallbackHandler : IEventHandler
        {
            Handlers.Add(handler);
        }
        
        public static void RemoveEmptyHandler<TCallbackHandler>(TCallbackHandler handler)
            where TCallbackHandler : IEventHandler
        {
            Handlers.RemoveWhere(h => h.GetType() == typeof(TCallbackHandler));
        }
        
        public static void Register<TEventType>(CallbackDelegate<TEventType> callback)
        where TEventType : struct, IEvent
        {
            EventHandler<TEventType>.RegisterCallback(callback);
        } 
        
        public static void Unregister<TEventType>(CallbackDelegate<TEventType> callback)
            where TEventType : struct, IEvent
        {
            EventHandler<TEventType>.UnregisterCallback(callback);
        }

        public static void Invoke<TDelegateType>(ref TDelegateType data)
            where TDelegateType : struct, IEvent
        {
            EventHandler<TDelegateType>.Invoke(ref data);
        }

        public static void Invoke<TDelegateType>()
            where TDelegateType : struct, IEvent
        {
            TDelegateType requestData = new TDelegateType();
            EventHandler<TDelegateType>.Invoke(ref requestData);
        }
        
        public static void Clear()
        {
            foreach (var handler in Handlers)
            {
                handler.Clear();
            }
            Handlers.Clear();
            Debug.Log($"{nameof(EventsAPI)} has been cleared");
        }
    }
}
