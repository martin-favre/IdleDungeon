using System.Collections.Generic;

public abstract class Character : ICharacter
{
    private readonly string name;
    protected IHealthPoints healthPoints;
    protected ICombatAttributes combatAttributes;
    private readonly double experienceWorth;
    private readonly IGuid guid;

    protected readonly List<ICharacterAction> characterActions;
    private readonly double goldWorth;
    protected Character(string name, IHealthPoints healthPoints, ICombatAttributes combatAttributes, double experienceWorth = 0, double goldWorth = 0)
    {
        this.name = name;
        this.healthPoints = healthPoints;
        this.combatAttributes = combatAttributes;
        this.experienceWorth = experienceWorth;
        this.goldWorth = goldWorth;
        this.guid = GuidProvider.Instance.GetNewGuid();
        this.characterActions = new List<ICharacterAction>();
    }

    public string Name => name;
    public ICombatAttributes Attributes => combatAttributes;
    public double ExperienceWorth => experienceWorth;
    public IGuid UniqueId => guid;
    public IHealthPoints HealthPoints => healthPoints;
    public ICharacterAction[] CharacterActions { get => characterActions.ToArray(); }
    public double GoldWorth => goldWorth;

    public void BeAttacked(double attackStat)
    {
        HealthPoints.Damage(attackStat);
    }
    public bool IsDead() { return HealthPoints.IsDead(); }
    public abstract void PerformAction(List<ICharacter> enemies, ICombatReader combat);

    protected void IncrementActions(ICombatReader combat)
    {
        foreach (var action in CharacterActions)
        {
            if (action.TurnProgress != null)
            {
                if (action.TurnProgress.IncrementTurnProgress(Attributes.Speed))
                {
                    action.PerformAction(this, combat);
                    action.PostAction();
                }
            }
        }

    }
}