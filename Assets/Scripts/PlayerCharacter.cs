using System.Collections.Generic;


// Represents a Character the player owns. i.e. not an enemy.
public class PlayerCharacter : ICombatant
{
    private readonly IRandomProvider random;
    private readonly IEventRecipient<PlayerCharacterUpdateEvent> playerEvRecipient;
    private readonly CombatAttributes attributes = new CombatAttributes();
    private readonly TurnProgress turnProgress = new TurnProgress();

    public CombatAttributes Attributes => attributes;

    public ITurnProgress TurnProgress => turnProgress;

    public PlayerCharacter(IRandomProvider random,
                            IEventRecipient<PlayerCharacterUpdateEvent> playerEvRecipient)
    {
        this.random = random;
        this.playerEvRecipient = playerEvRecipient;
    }

    public void PerformAction(List<ICombatant> enemies, ICombatReader combat, IEventRecipient<ICombatUpdateEvent> evRecipient)
    {
        if (enemies.Count == 0) return;
        ICombatant enemy = Helpers.GetRandom<ICombatant>(enemies, random);
        enemy.BeAttacked(attributes.Attack);
        evRecipient.RecieveEvent(new CombatActionEvent(combat, enemy, this));
    }

    public void BeAttacked(int attackStat)
    {
        int dmg = attributes.CalculateDamage(attackStat);
        attributes.Damage(dmg);
        playerEvRecipient.RecieveEvent(new PlayerCharacterUpdateEvent(this));
    }

    public bool IsDead()
    {
        return attributes.IsDead();
    }
}