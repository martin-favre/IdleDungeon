using PubSubSystem;

public interface ICombatUpdateEvent : IEvent
{
    ICombatReader Combat { get; }
}
