public interface ICombatUpdateEvent
{
    ICombatReader Combat { get; }
}


public class EnteredCombatEvent : ICombatUpdateEvent
{
    private readonly ICombatReader combat;

    public EnteredCombatEvent(ICombatReader combat)
    {
        this.combat = combat;
    }

    public ICombatReader Combat { get => combat; }
}
public class ExitedCombatEvent : ICombatUpdateEvent
{
    public enum CombatResult {
        PlayerWon,
        PlayerLost
    }
    private readonly ICombatReader combat;
    private readonly CombatResult result;

    public ExitedCombatEvent(ICombatReader combat, CombatResult result)
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

    public CombatActionEvent(ICombatReader combat, ICharacter target, ICharacter actionDoer)
    {
        this.combat = combat;
        this.target = target;
        this.actionDoer = actionDoer;
    }

    public ICharacter Target => target;
    public ICharacter ActionDoer => actionDoer;
    public ICombatReader Combat => combat;
}