using PubSubSystem;

public class PlayerClickedEnemyEvent : IEvent
{
    ICharacter enemy;
    public PlayerClickedEnemyEvent(ICharacter enemy)
    {
        this.enemy = enemy;
    }
    public ICharacter Enemy { get => enemy; set => enemy = value; }
}
