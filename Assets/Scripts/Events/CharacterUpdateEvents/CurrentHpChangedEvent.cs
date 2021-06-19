public class CurrentHpChanged : CharacterUpdateEvent
{
    private readonly double healthChange;


    public CurrentHpChanged(double healthChange, ICharacter character) : base(character)
    {
        this.healthChange = healthChange;

    }

    public double HealthChange => healthChange;
}
