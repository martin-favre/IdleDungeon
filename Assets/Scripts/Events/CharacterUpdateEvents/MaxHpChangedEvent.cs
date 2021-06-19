public class MaxHpChangedEvent : CharacterUpdateEvent
{
    private readonly double healthChange;


    public MaxHpChangedEvent(double healthChange, ICharacter character) : base(character)
    {
        this.healthChange = healthChange;

    }

    public double HealthChange => healthChange;
}
