
using PubSubSystem;

public class BuffAppliedEvent : IEvent
{
    private readonly ICharacter eventOwner;
    private readonly IBuff appliedBuff;

    public BuffAppliedEvent(ICharacter eventOwner, IBuff appliedBuff)
    {
        this.eventOwner = eventOwner;
        this.appliedBuff = appliedBuff;
    }

    public ICharacter EventOwner => eventOwner;

    public IBuff AppliedBuff => appliedBuff;

}