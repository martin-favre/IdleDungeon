using System;
using System.Collections.Generic;
using UnityEngine;

class LevelGeneratedCharacter : ICharacter, IEventRecipient<ICharacterUpdateEvent>, IHasMaterial
{
    private readonly LevelGeneratedCombatAttributes attributes;
    private readonly TurnProgress turnProgress = new TurnProgress();

    private readonly Guid guid = Guid.NewGuid();

    public ICombatAttributes Attributes => attributes;

    public ITurnProgress TurnProgress => turnProgress;

    public Guid UniqueId => guid;

    readonly double experienceWorth;
    private readonly EnemyTemplate template;

    public double ExperienceWorth => experienceWorth;

    public string Name => template.Name;

    public Material Material => template.Material;

    List<IObserver<ICharacterUpdateEvent>> observers = new List<IObserver<ICharacterUpdateEvent>>();

    public LevelGeneratedCharacter(EnemyTemplate template, int currentLevel, float powerFactor)
    {
        attributes = new LevelGeneratedCombatAttributes(currentLevel, powerFactor);
        experienceWorth = powerFactor*(10 + Mathf.RoundToInt(10 * Mathf.Pow(1.07f, (float)currentLevel)));
        this.template = template;
    }

    public void BeAttacked(double attackStat)
    {
        attributes.Damage(attackStat);
    }

    public bool IsDead()
    {
        return attributes.IsDead();
    }

    public void PerformAction(List<ICharacter> enemies, ICombatReader combat, IEventRecipient<ICombatUpdateEvent> evRecipient)
    {
        if (enemies.Count == 0) return;
        ICharacter target = enemies[SingletonProvider.MainRandomProvider.RandomInt(0, enemies.Count)];
        target.BeAttacked(attributes.Attack);
        evRecipient.RecieveEvent(new CombatActionEvent(combat, target, this));
    }

    public IDisposable Subscribe(IObserver<ICharacterUpdateEvent> observer)
    {
        return new SimpleUnsubscriber<ICharacterUpdateEvent>(observers, observer);
    }

    public void RecieveEvent(ICharacterUpdateEvent ev)
    {
        observers.ForEach(o => o.OnNext(ev));
    }
}
