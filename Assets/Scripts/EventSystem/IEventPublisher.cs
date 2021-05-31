namespace PubSubSystem
{
    public interface IEventPublisher<KeyType>
    {
        Subscription<KeyType> Subscribe(KeyType key, System.Action<IEvent> handler);
        Subscription<KeyType> Subscribe(KeyType[] key, System.Action<IEvent> handler);
        void Unsubscribe(Subscription<KeyType> hook);
        void Publish(KeyType key, IEvent ev);
    }
}