using System.Collections.Generic;

public abstract class Character : ICharacter
{
    private readonly string name;
    protected IHealthPoints healthPoints;
    protected ICombatAttributes combatAttributes;
    private readonly double experienceWorth;
    private readonly IGuid guid;

    protected readonly List<ICharacterAction> possibleCharacterActions;
    protected ICharacterAction activeCharacterAction;
    private readonly double goldWorth;
    protected Character(string name, ICombatAttributes combatAttributes, double experienceWorth = 0, double goldWorth = 0)
    {
        this.name = name;
        this.combatAttributes = combatAttributes;
        this.experienceWorth = experienceWorth;
        this.goldWorth = goldWorth;
        this.guid = GuidProvider.Instance.GetNewGuid();
        this.possibleCharacterActions = new List<ICharacterAction>();
    }

    public string Name => name;
    public ICombatAttributes Attributes => combatAttributes;
    public double ExperienceWorth => experienceWorth;
    public IGuid UniqueId => guid;
    public IHealthPoints HealthPoints => healthPoints;
    public ICharacterAction[] CharacterActions { get => possibleCharacterActions.ToArray(); }
    public double GoldWorth => goldWorth;

    public void BeAttacked(double attackStat)
    {
        HealthPoints.Damage(attackStat);
    }
    public bool IsDead() { return HealthPoints.IsDead(); }
    public abstract void PerformAction(List<ICharacter> enemies, ICombatReader combat);

    protected void StartChargingAction(ICharacterAction action, ICharacter target, ICombatReader combat)
    {
        if (activeCharacterAction != null)
        {
            var prevAction = activeCharacterAction;
            activeCharacterAction.CancelAction();
            SingletonProvider.MainEventPublisher.Publish(EventType.CharacterActionCancelled, new CharacterActionCancelledEvent(this, prevAction));
        }
        action.StartChargingAction(this, target, combat);
        activeCharacterAction = action;
    }

    protected void IncrementActions(ICombatReader combat)
    {
        if (activeCharacterAction != null)
        {
            if (activeCharacterAction.TurnProgress.IncrementTurnProgress(Attributes.Speed))
            {
                activeCharacterAction.PerformAction(this, combat);
                activeCharacterAction.PostAction();
                activeCharacterAction = null;
            }
        }
    }
}