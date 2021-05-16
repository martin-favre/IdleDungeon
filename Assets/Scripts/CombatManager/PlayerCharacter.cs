using System;
using System.Collections.Generic;

public class PlayerCharacterAttributeUpdateEvent : IPlayerRosterUpdateEvent
{
    private readonly ICombatAttributes attributes;

    public PlayerCharacterAttributeUpdateEvent(ICombatAttributes attributes)
    {
        this.attributes = attributes;
    }

    public ICombatAttributes Attributes => attributes;
}

// Represents a Character the player owns. i.e. not an enemy.
public class PlayerCharacter : ICharacter
{
    private readonly IRandomProvider random;
    private readonly IEventRecipient<IPlayerRosterUpdateEvent> playerEvRecipient;
    private readonly PlayerAttributes attributes;
    private readonly TurnProgress turnProgress = new TurnProgress();
    private Guid guid = Guid.NewGuid();

    public ICombatAttributes Attributes => attributes;

    public ITurnProgress TurnProgress => turnProgress;

    public Guid UniqueId => guid;

    public double ExperienceWorth => 0; // Players are not worth experience :D

    public PlayerCharacter(IRandomProvider random,
                            IEventRecipient<IPlayerRosterUpdateEvent> playerEvRecipient, IUpgradeManager upgradeManager, int playerIdentifier)
    {
        this.random = random;
        this.playerEvRecipient = playerEvRecipient;
        attributes = new PlayerAttributes(PlayerPrefsReader.Instance, playerEvRecipient, upgradeManager, playerIdentifier);
    }

    public void PerformAction(List<ICharacter> enemies, ICombatReader combat, IEventRecipient<ICombatUpdateEvent> evRecipient)
    {
        if (enemies.Count == 0) return;
        ICharacter enemy = Helpers.GetRandom<ICharacter>(enemies, random);
        enemy.BeAttacked(attributes.Attack);
        evRecipient.RecieveEvent(new CombatActionEvent(combat, enemy, this));
    }

    public void BeAttacked(double attackStat)
    {
        attributes.Damage(attackStat);
    }

    public bool IsDead()
    {
        return attributes.IsDead();
    }
}