using System;
using System.Collections.Generic;

namespace EventSystem
{
    public class EventManager : IEventManager
    {
        static readonly IEventManager instance;
        Dictionary<string, List<Action<IEvent>>> handlers = new Dictionary<string, List<Action<IEvent>>>();

        public static IEventManager Instance => instance;

        static EventManager()
        {
            instance = new EventManager();
        }

        public EventHook StartListen(string key, Action<IEvent> handler)
        {
            List<Action<IEvent>> list;
            bool success = handlers.TryGetValue(key, out list);
            if (!success)
            {
                list = new List<Action<IEvent>>();
                handlers[key] = list;
            }
            list.Add(handler);
            return new EventHook(key, handler);
        }

        public void StopListen(EventHook hook)
        {
            List<Action<IEvent>> list;
            bool success = handlers.TryGetValue(hook.Key, out list);
            if (!success) return;
            list.Remove(hook.Handler);
        }

        public void TriggerEvent(string key, IEvent ev)
        {
            List<Action<IEvent>> list;
            bool success = handlers.TryGetValue(key, out list);
            if (!success) return;
            list.ForEach(e => e(ev));
        }
    }
}