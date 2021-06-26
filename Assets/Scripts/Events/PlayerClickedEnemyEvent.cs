using PubSubSystem;

public class PlayerClickedTargetEvent : IEvent
{
    ICharacter enemy;
    public PlayerClickedTargetEvent(ICharacter enemy)
    {
        this.enemy = enemy;
    }
    public ICharacter Enemy { get => enemy; set => enemy = value; }
}
