
public class CombatUpdateEvent : ICombatUpdateEvent
{
    private readonly ICombatReader combat;

    public CombatUpdateEvent(ICombatReader combat)
    {
        this.combat = combat;
    }

    public ICombatReader Combat => combat;

}
