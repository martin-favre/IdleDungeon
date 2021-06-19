using PubSubSystem;

public class CombatResultsClosedEvent : IEvent
{
    private readonly CombatResult result;

    public CombatResultsClosedEvent(CombatResult result)
    {
        this.result = result;
    }

    public CombatResult Result => result;
}