
using PubSubSystem;

public class BuffExpiredEvent : IEvent {
    private readonly ICharacter eventOwner;
    private readonly IBuff expiredBuff;

    public BuffExpiredEvent(ICharacter eventOwner, IBuff expiredBuff)
    {
        this.eventOwner = eventOwner;
        this.expiredBuff = expiredBuff;
    }

    public ICharacter EventOwner => eventOwner;

    public IBuff ExpiredBuff => expiredBuff;
}