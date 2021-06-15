using System;
using System.Collections.Generic;
using UnityEngine;

class LevelGeneratedCharacter : Character, IHasMaterial, IHasEnemyTemplate
{
    private readonly LevelGeneratedCombatAttributes attributes;
    private readonly TurnProgress turnProgress = new TurnProgress();

    private readonly EnemyTemplate template;

    public Material Material => template.Material;

    public EnemyTemplate EnemyTemplate => template;

    public LevelGeneratedCharacter(EnemyTemplate template, int currentLevel, float powerFactor) : base(template.Name, null, new LevelGeneratedCombatAttributes(currentLevel, powerFactor), 200)
    {
        healthPoints = new HealthPoints(new WeakReference<ICharacter>(this), 200);
        this.template = template;
        base.characterActions.Add(new AttackRandomAction("Sprites/Slime", "Attack"));
    }

    public override void PerformAction(List<ICharacter> enemies, ICombatReader combat)
    {
        if (characterActions.TrueForAll(e => e.TurnProgress == null))
        {
            characterActions[0].StartChargingAction(this, combat);
        }
        IncrementActions(combat);
    }
}
