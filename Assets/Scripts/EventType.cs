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
    PlayerSelectedActionTarget,
    PlayerClickedEnemy,
    PlayerClickedNothing,
    CombatResultsClosed
}


public class PlayerClickedEnemyEvent : IEvent
{
    ICharacter enemy;
    public PlayerClickedEnemyEvent(ICharacter enemy)
    {
        this.enemy = enemy;
    }
    public ICharacter Enemy { get => enemy; set => enemy = value; }
}
public class PlayerClickedNothingEvent : IEvent { }

public class CombatResultsClosedEvent : IEvent
{
    private readonly CombatResult result;

    public CombatResultsClosedEvent(CombatResult result)
    {
        this.result = result;
    }

    public CombatResult Result => result;
}