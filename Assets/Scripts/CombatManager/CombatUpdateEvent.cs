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
    private readonly ICombatReader combat;

    public ExitedCombatEvent(ICombatReader combat)
    {
        this.combat = combat;
    }

    public ICombatReader Combat { get => combat; }

}

public class CombatantDied : ICombatUpdateEvent
{
    private readonly ICombatReader combat;
    private readonly ICombatant victim;

    public CombatantDied(ICombatReader combat, ICombatant victim)
    {
        this.combat = combat;
        this.victim = victim;
    }

    public ICombatReader Combat => combat;

    public ICombatant Victim => victim;
}

public class CombatActionEvent : ICombatUpdateEvent
{
    private readonly ICombatReader combat;
    private readonly ICombatant target;
    private readonly ICombatant actionDoer;

    public CombatActionEvent(ICombatReader combat, ICombatant target, ICombatant actionDoer)
    {
        this.combat = combat;
        this.target = target;
        this.actionDoer = actionDoer;
    }

    public ICombatant Target => target;
    public ICombatant ActionDoer => actionDoer;
    public ICombatReader Combat => combat;
}