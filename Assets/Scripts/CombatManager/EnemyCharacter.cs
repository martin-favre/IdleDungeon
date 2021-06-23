using System;
using System.Collections.Generic;



public class EnemyCharacter : Character, IHasEnemyTemplate
{
    private readonly EnemyTemplate template;
    Dictionary<ICharacterAction, float> nextActionTime = new Dictionary<ICharacterAction, float>(); // hack to make characters sorta feel like they take time thinking about their actions
    float actionCooldownS = 2;

    public EnemyCharacter(EnemyTemplate template) :


    base(template.Name, new EnemyAttributes(template.Attack, template.Speed), GetExperienceWorth(template.WorthFactor), GetGoldWorth(template.WorthFactor))
    {
        this.possibleCharacterActions.AddRange(template.Factory.SpawnActions());
        this.healthPoints = new HealthPoints(new WeakReference<ICharacter>(this), template.MaxHp);
        this.template = template;
        base.possibleCharacterActions.ForEach(e => nextActionTime[e] = SingletonProvider.MainTimeProvider.Time + actionCooldownS * SingletonProvider.MainRandomProvider.RandomFloat(0.5f, 1.5f));

    }

    static private double GetExperienceWorth(double powerFactor)
    {
        return 100 * powerFactor;
    }
    static private double GetGoldWorth(double powerFactor)
    {
        return GetExperienceWorth(powerFactor); // idk can scale the same for now
    }
    public EnemyTemplate EnemyTemplate => template;

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