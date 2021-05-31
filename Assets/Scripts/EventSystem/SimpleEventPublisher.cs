using System;

namespace PubSubSystem
{
    public class SimpleEventPublisher : EventPublisher<int>
    {
        public Subscription<int> Subscribe(Action<IEvent> handler)
        {
            return base.Subscribe(0, handler);
        }

        public void Publish(IEvent ev)
        {
            base.Publish(0, ev);
        }
    }
}