using PubSubSystem;


public class CentralEventHandler : EventPublisher<EventType>
{
    static readonly CentralEventHandler instance;

    public static CentralEventHandler Instance => instance;

    static CentralEventHandler()
    {
        instance = new CentralEventHandler();
    }
}