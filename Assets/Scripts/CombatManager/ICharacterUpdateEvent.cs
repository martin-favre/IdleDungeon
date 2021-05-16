public interface ICharacterUpdateEvent
{

}

public class NameUpdateEvent : ICharacterUpdateEvent
{
}

public class AttributeChangedEvent : ICharacterUpdateEvent
{
    private readonly PlayerAttributes playerAttributes;

    public AttributeChangedEvent(PlayerAttributes playerAttributes)
    {
        this.playerAttributes = playerAttributes;
    }

    public PlayerAttributes PlayerAttributes => playerAttributes;
}