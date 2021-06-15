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
    PlayerClickedNothing
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