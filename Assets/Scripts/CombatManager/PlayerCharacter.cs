using System;
using System.Collections.Generic;

public class PlayerCharacterAttributeUpdateEvent : IPlayerCharacterUpdateEvent
{
    private readonly ICombatAttributes attributes;

    public PlayerCharacterAttributeUpdateEvent(ICombatAttributes attributes)
    {
        this.attributes = attributes;
    }

    public ICombatAttributes Attributes => attributes;
}

// Represents a Character the player owns. i.e. not an enemy.
public class PlayerCharacter : ICombatant
{
    private readonly IRandomProvider random;
    private readonly IEventRecipient<IPlayerCharacterUpdateEvent> playerEvRecipient;
    private readonly PlayerAttributes attributes;
    private readonly TurnProgress turnProgress = new TurnProgress();

    private Guid guid = Guid.NewGuid();

    public ICombatAttributes Attributes => attributes;

    public ITurnProgress TurnProgress => turnProgress;

    public Guid UniqueId => guid;

    public double ExperienceWorth => 0; // Players are not worth experience :D

    public PlayerCharacter(IRandomProvider random,
                            IEventRecipient<IPlayerCharacterUpdateEvent> playerEvRecipient)
    {
        this.random = random;
        this.playerEvRecipient = playerEvRecipient;
        attributes = new PlayerAttributes(PlayerPrefsReader.Instance, playerEvRecipient, UpgradeManager.Instance);
    }

    public void PerformAction(List<ICombatant> enemies, ICombatReader combat, IEventRecipient<ICombatUpdateEvent> evRecipient)
    {
        if (enemies.Count == 0) return;
        ICombatant enemy = Helpers.GetRandom<ICombatant>(enemies, random);
        enemy.BeAttacked(attributes.Attack);
        evRecipient.RecieveEvent(new CombatActionEvent(combat, enemy, this));
    }

    public void BeAttacked(double attackStat)
    {
        attributes.Damage(attackStat);
        playerEvRecipient.RecieveEvent(new PlayerCharacterAttributeUpdateEvent(attributes));
    }

    public bool IsDead()
    {
        return attributes.IsDead();
    }
}