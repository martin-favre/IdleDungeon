using System;
using System.Collections.Generic;



public class EnemyCharacter : Character, IHasEnemyTemplate
{
    private readonly EnemyTemplate template;
    SimleTimer nextActionCooldown;
    public EnemyCharacter(EnemyTemplate template) :


    base(template.Name, new EnemyAttributes(template.Attack, template.Speed), GetExperienceWorth(template.WorthFactor), GetGoldWorth(template.WorthFactor))
    {
        this.possibleCharacterActions.AddRange(template.Factory.SpawnActions());
        this.healthPoints = new HealthPoints(new WeakReference<ICharacter>(this), template.MaxHp);
        this.template = template;
        nextActionCooldown = new SimleTimer(SingletonProvider.MainRandomProvider.RandomFloat(0.5f, 1.5f));
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
            if (nextActionCooldown.IsDone())
            {
                StartChargingAction(possibleCharacterActions[0], null, combat);
                nextActionCooldown = new SimleTimer(SingletonProvider.MainRandomProvider.RandomFloat(0.5f, 1.5f) + possibleCharacterActions[0].TurnProgress.TurnTime);
            }
        }
        IncrementActions(combat);
    }
}