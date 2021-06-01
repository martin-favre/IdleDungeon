
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

public class NameUpdateEvent : CharacterUpdateEvent
{
    public NameUpdateEvent(ICharacter character) : base(character)
    {
    }
}

public class AttributeChangedEvent : CharacterUpdateEvent
{
    public AttributeChangedEvent(ICharacter character) : base(character)
    {
    }
}

public class CurrentHpChanged : CharacterUpdateEvent
{
    private readonly double healthChange;


    public CurrentHpChanged(double healthChange, ICharacter character) : base(character)
    {
        this.healthChange = healthChange;

    }
    
    public double HealthChange => healthChange;

}
