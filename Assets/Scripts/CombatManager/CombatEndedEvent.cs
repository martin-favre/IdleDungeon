public class CombatEndedEvent : ICombatUpdateEvent
{
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
