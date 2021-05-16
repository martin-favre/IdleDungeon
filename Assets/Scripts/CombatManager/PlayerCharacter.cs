using System;
using System.Collections.Generic;

// Represents a Character the player owns. i.e. not an enemy.
public class PlayerCharacter : ICharacter, IEventRecipient<ICharacterUpdateEvent>
{
    private readonly IRandomProvider random;
    private readonly PlayerAttributes attributes;
    private readonly TurnProgress turnProgress = new TurnProgress();
    private Guid guid = Guid.NewGuid();

    public ICombatAttributes Attributes => attributes;

    public ITurnProgress TurnProgress => turnProgress;

    public Guid UniqueId => guid;

    public double ExperienceWorth => 0; // Players are not worth experience :D

    List<IObserver<ICharacterUpdateEvent>> observers = new List<IObserver<ICharacterUpdateEvent>>();

    public PlayerCharacter(IRandomProvider random, IUpgradeManager upgradeManager, int playerIdentifier)
    {
        this.random = random;
        attributes = new PlayerAttributes(PlayerPrefsReader.Instance, this, upgradeManager, playerIdentifier);
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

    public IDisposable Subscribe(IObserver<ICharacterUpdateEvent> observer)
    {
        return new SimpleUnsubscriber<ICharacterUpdateEvent>(observers, observer);
    }

    public void RecieveEvent(ICharacterUpdateEvent ev)
    {
        var oldObservers = observers.ToArray();
        foreach (var observer in oldObservers) { observer.OnNext(ev); }
    }
}