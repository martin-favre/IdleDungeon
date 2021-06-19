public class CombatActionEvent : CombatUpdateEvent
{
    private readonly ICharacter target;
    private readonly ICharacter actionDoer;
    private readonly ICharacterAction action;

    public CombatActionEvent(ICombatReader combat, ICharacter target, ICharacter actionDoer, ICharacterAction action) : base(combat)
    {
        this.target = target;
        this.actionDoer = actionDoer;
        this.action = action;
    }

    public ICharacter Target => target;
    public ICharacter ActionDoer => actionDoer;
    public ICharacterAction Action => action;
}