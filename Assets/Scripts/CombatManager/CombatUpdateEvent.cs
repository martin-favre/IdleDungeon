using PubSubSystem;

public interface ICombatUpdateEvent : IEvent
{
    ICombatReader Combat { get; }
}


public class CombatStartedEvent : ICombatUpdateEvent
{
    private readonly ICombatReader combat;

    public CombatStartedEvent(ICombatReader combat)
    {
        this.combat = combat;
    }

    public ICombatReader Combat { get => combat; }
}
public class CombatEndedEvent : ICombatUpdateEvent
{
    public enum CombatResult {
        PlayerWon,
        PlayerLost
    }
    private readonly ICombatReader combat;
    private readonly CombatResult result;

    public CombatEndedEvent(ICombatReader combat, CombatResult result)
    {
        this.combat = combat;
        this.result = result;
    }

    public ICombatReader Combat { get => combat; }

    public CombatResult Result => result;
}

public class CombatantDied : ICombatUpdateEvent
{
    private readonly ICombatReader combat;
    private readonly ICharacter victim;

    public CombatantDied(ICombatReader combat, ICharacter victim)
    {
        this.combat = combat;
        this.victim = victim;
    }

    public ICombatReader Combat => combat;

    public ICharacter Victim => victim;
}

public class CombatActionEvent : ICombatUpdateEvent
{
    private readonly ICombatReader combat;
    private readonly ICharacter target;
    private readonly ICharacter actionDoer;
    private readonly ICharacterAction action;

    public CombatActionEvent(ICombatReader combat, ICharacter target, ICharacter actionDoer, ICharacterAction action)
    {
        this.combat = combat;
        this.target = target;
        this.actionDoer = actionDoer;
        this.action = action;
    }

    public ICharacter Target => target;
    public ICharacter ActionDoer => actionDoer;
    public ICombatReader Combat => combat;
    public ICharacterAction Action => action;
}