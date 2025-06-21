namespace Terra.EventsSystem
{
    public interface IEvent { }
    
    public delegate void CallbackDelegate<TEvent>(ref TEvent type) where TEvent : struct, IEvent;
    
}