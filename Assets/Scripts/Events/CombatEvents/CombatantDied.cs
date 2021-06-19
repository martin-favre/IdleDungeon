public class CombatantDied : CombatUpdateEvent
{
    private readonly ICharacter victim;

    public CombatantDied(ICombatReader combat, ICharacter victim) : base(combat)
    {
        this.victim = victim;
    }


    public ICharacter Victim => victim;
}

