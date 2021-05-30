using System;

namespace EventSystem
{
    public class EventHook : System.IDisposable
    {
        private readonly string key;
        private readonly Action<IEvent> handler;

        public EventHook(string key, Action<IEvent> handler)
        {
            this.key = key;
            this.handler = handler;
        }

        public string Key => key;

        public Action<IEvent> Handler => handler;

        public void Dispose()
        {
            EventManager.Instance.StopListen(this);
        }
    }

}