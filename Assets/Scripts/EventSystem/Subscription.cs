using System;

namespace PubSubSystem
{
    public class Subscription<KeyType> : System.IDisposable
    {
        private readonly KeyType[] keys;
        private readonly Action<IEvent> handler;
        private readonly IEventPublisher<KeyType> eventManager;

        public Subscription(KeyType[] keys, Action<IEvent> handler, IEventPublisher<KeyType> eventManager)
        {
            this.keys = keys;
            this.handler = handler;
            this.eventManager = eventManager;
        }

        public KeyType[] Keys => keys;

        public Action<IEvent> Handler => handler;

        public void Dispose()
        {
            eventManager.Unsubscribe(this);
        }
    }

}