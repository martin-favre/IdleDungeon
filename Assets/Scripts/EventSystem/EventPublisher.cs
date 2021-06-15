using System;
using System.Collections.Generic;

namespace PubSubSystem
{
    public class EventPublisher<KeyType> : IEventPublisher<KeyType>
    {
        Dictionary<KeyType, List<Action<IEvent>>> handlers = new Dictionary<KeyType, List<Action<IEvent>>>();
        public Subscription<KeyType> Subscribe(KeyType key, Action<IEvent> handler)
        {
            return Subscribe(new KeyType[] { key }, handler);
        }
        public Subscription<KeyType> Subscribe(KeyType[] keys, Action<IEvent> handler)
        {
            foreach (var key in keys)
            {
                List<Action<IEvent>> list;
                bool success = handlers.TryGetValue(key, out list);
                if (!success)
                {
                    list = new List<Action<IEvent>>();
                    handlers[key] = list;
                }
                list.Add(handler);
            }
            return new Subscription<KeyType>(keys, handler, this);
        }

        public void Unsubscribe(Subscription<KeyType> hook)
        {
            foreach (var key in hook.Keys)
            {
                List<Action<IEvent>> list;
                bool success = handlers.TryGetValue(key, out list);
                if (!success) return;
                list.Remove(hook.Handler);
            }
        }

        public void Publish(KeyType key, IEvent ev)
        {
            List<Action<IEvent>> list;
            bool success = handlers.TryGetValue(key, out list);
            if (!success) return;
            var pubCopy = list.ToArray(); // To allow subs/unsubs when handling events
            foreach (var e in pubCopy)
            {
                e(ev);
            }
        }
    }
}