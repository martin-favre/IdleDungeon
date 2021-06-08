using System;
using System.Collections.Generic;
using UnityEngine;

class LevelGeneratedCharacter : ICharacter, IHasMaterial
{
    private readonly LevelGeneratedCombatAttributes attributes;
    private readonly TurnProgress turnProgress = new TurnProgress();

    private readonly IGuid guid = GuidProvider.Instance.GetNewGuid();

    public ICombatAttributes Attributes => attributes;

    public ITurnProgress TurnProgress => turnProgress;

    public IGuid UniqueId => guid;

    readonly double experienceWorth;
    private readonly EnemyTemplate template;

    public double ExperienceWorth => experienceWorth;

    public string Name => template.Name;

    public Material Material => template.Material;

    public LevelGeneratedCharacter(EnemyTemplate template, int currentLevel, float powerFactor)
    {
        attributes = new LevelGeneratedCombatAttributes(currentLevel, powerFactor, this);
        experienceWorth = powerFactor*(100 + Mathf.RoundToInt(100 * Mathf.Pow(1.07f, (float)currentLevel)));
        this.template = template;
        this.turnProgress.RandomizeProgress();
    }

    public void BeAttacked(double attackStat)
    {
        attributes.Damage(attackStat);
    }

    public bool IsDead()
    {
        return attributes.IsDead();
    }

    public void PerformAction(List<ICharacter> enemies, ICombatReader combat)
    {
        if (enemies.Count == 0) return;
        ICharacter target = enemies[SingletonProvider.MainRandomProvider.RandomInt(0, enemies.Count)];
        target.BeAttacked(attributes.Attack);
        MainEventHandler.Instance.Publish(EventType.CombatAction, new CombatActionEvent(combat, target, this));
    }

    public void Dispose()
    {
        attributes.Dispose();
    }
}
