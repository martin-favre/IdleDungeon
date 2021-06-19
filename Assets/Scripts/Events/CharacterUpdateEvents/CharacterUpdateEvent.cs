using PubSubSystem;

public class CharacterUpdateEvent : IEvent
{
    private readonly ICharacter character;

    public CharacterUpdateEvent(ICharacter character)
    {
        this.character = character;
    }

    public ICharacter Character => character;
}
