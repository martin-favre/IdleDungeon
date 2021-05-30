namespace EventSystem
{
    public interface IEventManager
    {
        EventHook StartListen(string key, System.Action<IEvent> handler);
        void StopListen(EventHook hook);
        void TriggerEvent(string key, IEvent ev);
    }
}