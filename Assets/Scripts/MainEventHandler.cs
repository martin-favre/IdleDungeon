using PubSubSystem;

public enum EventType
{
    CharacterNameUpdated,
    CharacterAttributeChanged,
    CharacterCurrentHpChanged,
    CharacterMaxHpChanged,
    CombatStarted,
    CombatEnded,
    CombatAction,
    CombatantDied,
}

public class CentralEventHandler : EventPublisher<EventType>
{
    static readonly CentralEventHandler instance;

    public static CentralEventHandler Instance => instance;

    static CentralEventHandler()
    {
        instance = new CentralEventHandler();
    }
}