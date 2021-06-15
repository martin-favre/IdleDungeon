using PubSubSystem;

public class PlayerSelectedActionTargetEvent : IEvent
{
    private readonly ICharacter user;
    private readonly ICharacter target;
    private readonly ICharacterAction action;

    public PlayerSelectedActionTargetEvent(ICharacter user, ICharacter target, ICharacterAction action)
    {
        this.user = user;
        this.action = action;
        this.target = target;
    }

    public ICharacter User => user;

    public ICharacterAction Action => action;

    public ICharacter Target => target;
}