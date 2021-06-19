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

    Dictionary<ICharacterAction, float> nextActionTime = new Dictionary<ICharacterAction, float>(); // hack to make characters sorta feel like they take time thinking about their actions
    float actionCooldownS = 2;

    public LevelGeneratedCharacter(EnemyTemplate template, int currentLevel, float powerFactor) : base(template.Name, null, new LevelGeneratedCombatAttributes(currentLevel, powerFactor), 200, 100)
    {
        healthPoints = new HealthPoints(new WeakReference<ICharacter>(this), 200);
        this.template = template;
        base.possibleCharacterActions.Add(new AttackRandomAction("Sprites/addon_04", "Attack"));
        base.possibleCharacterActions.ForEach(e => nextActionTime[e] = SingletonProvider.MainTimeProvider.Time + actionCooldownS * SingletonProvider.MainRandomProvider.RandomFloat(0.5f, 1.5f));
    }

    public override void PerformAction(List<ICharacter> enemies, ICombatReader combat)
    {
        if (possibleCharacterActions.TrueForAll(e => e.TurnProgress == null))
        {
            if (nextActionTime[possibleCharacterActions[0]] < SingletonProvider.MainTimeProvider.Time)
            {
                StartChargingAction(possibleCharacterActions[0], null, combat);
                nextActionTime[possibleCharacterActions[0]] = SingletonProvider.MainTimeProvider.Time + actionCooldownS * SingletonProvider.MainRandomProvider.RandomFloat(0.5f, 1.5f);
            }
        }
        IncrementActions(combat);
    }
}
