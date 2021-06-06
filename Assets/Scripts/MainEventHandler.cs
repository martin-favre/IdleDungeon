using PubSubSystem;

public enum EventType
{
    CharacterNameUpdated,
    CharacterAttributeChanged,
    CharacterCurrentHpChanged,
    CombatStarted,
    CombatEnded,
    CombatAction,
    CombatantDied,
}

public class MainEventHandler : EventPublisher<EventType>
{
    static readonly MainEventHandler instance;

    public static MainEventHandler Instance => instance;

    static MainEventHandler()
    {
        instance = new MainEventHandler();
    }
}