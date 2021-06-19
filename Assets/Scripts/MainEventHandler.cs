using PubSubSystem;


public class CentralEventPublisher : EventPublisher<EventType>
{
    static readonly CentralEventPublisher instance;

    public static CentralEventPublisher Instance => instance;

    static CentralEventPublisher()
    {
        instance = new CentralEventPublisher();
    }
}