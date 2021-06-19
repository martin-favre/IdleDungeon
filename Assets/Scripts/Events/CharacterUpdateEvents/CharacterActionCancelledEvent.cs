
public class CharacterActionCancelledEvent : CharacterUpdateEvent
{
    private readonly ICharacterAction action;
    public CharacterActionCancelledEvent(ICharacter character, ICharacterAction action) : base(character)
    {
        this.action = action;
    }

    public ICharacterAction Action => action;
}
