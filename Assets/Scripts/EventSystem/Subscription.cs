using System;

namespace PubSubSystem
{
    public class Subscription<KeyType> : System.IDisposable
    {
        private KeyType[] keys;
        private Action<IEvent> handler;
        private IEventPublisher<KeyType> eventManager;

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
            handler = null;
            eventManager = null;
            keys = null;
        }
    }

}