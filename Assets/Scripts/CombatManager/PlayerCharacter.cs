using System;
using System.Collections.Generic;

// Represents a Character the player owns. i.e. not an enemy.
public class PlayerCharacter : ICharacter
{
    static readonly string[] names = new string[] {
        "Steve",
        "Bob",
        "Joe",
        "Eric"
    };

    private readonly IRandomProvider random;
    private readonly int playerIdentifier;
    private readonly PlayerAttributes attributes;
    private readonly TurnProgress turnProgress = new TurnProgress();
    private IGuid guid = GuidProvider.Instance.GetNewGuid();

    public ICombatAttributes Attributes => attributes;

    public ITurnProgress TurnProgress => turnProgress;

    public IGuid UniqueId => guid;

    public double ExperienceWorth => 0; // Players are not worth experience :D

    public string Name => names[playerIdentifier];

    public PlayerCharacter(int playerIdentifier)
    {
        
        this.playerIdentifier = playerIdentifier;
        attributes = new PlayerAttributes(playerIdentifier, this);
        turnProgress.RandomizeProgress();
    }

    public void PerformAction(List<ICharacter> enemies, ICombatReader combat)
    {
        if (enemies.Count == 0) return;
        ICharacter enemy = Helpers.GetRandom<ICharacter>(enemies, SingletonProvider.MainRandomProvider);
        enemy.BeAttacked(attributes.Attack);
        CombatEventPublisher.Instance.Publish(new CombatActionEvent(combat, enemy, this));
    }

    public void BeAttacked(double attackStat)
    {
        attributes.Damage(attackStat);
    }

    public bool IsDead()
    {
        return attributes.IsDead();
    }

    public void Dispose()
    {
        attributes.Dispose();
    }
}