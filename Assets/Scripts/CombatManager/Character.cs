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
    readonly List<IBuff> buffs = new List<IBuff>();
    public string Name => name;
    public ICombatAttributes Attributes => combatAttributes;
    public double ExperienceWorth => experienceWorth;
    public IGuid UniqueId => guid;
    public IHealthPoints HealthPoints => healthPoints;
    public ICharacterAction[] CharacterActions { get => possibleCharacterActions.ToArray(); }
    public double GoldWorth => goldWorth;
    public IBuff[] Buffs => buffs.ToArray();

    public float EffectiveAttack
    {
        get
        {
            float val = Attributes.Attack;
            buffs.ForEach(b => val = b.GetAttack(val));
            return val;
        }
    }

    public float EffectiveSpeed
    {
        get
        {
            float val = Attributes.Speed;
            buffs.ForEach(b => val = b.GetSpeed(val));
            return val;
        }
    }

    public abstract bool IsPlayer { get; }

    public void BeAttacked(ICharacter assailant, AttackConfig attack)
    {
        buffs.ForEach(b => attack = b.OnCharacterAttacked(this, assailant, attack));
        HealthPoints.Damage(attack.Damage);
    }

    public bool IsDead() { return HealthPoints.IsDead(); }
    public abstract void PerformAction(List<ICharacter> enemies, ICombatReader combat);

    protected virtual void StartChargingAction(ICharacterAction action, ICharacter target, ICombatReader combat)
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

    private void RemoveExpiredBuffs()
    {
        List<IBuff> removedBuffs = new List<IBuff>();
        foreach (var buff in buffs)
        {
            if (buff.IsExpired())
            {
                removedBuffs.Add(buff);
            }
        }
        buffs.RemoveAll(e => e.IsExpired());
        removedBuffs.ForEach(e => SingletonProvider.MainEventPublisher.Publish(EventType.BuffExpired, new BuffExpiredEvent(this, e)));
    }

    protected void IncrementActions(ICombatReader combat)
    {
        RemoveExpiredBuffs();
        if (activeCharacterAction != null && activeCharacterAction.TurnProgress != null)
        {
            if (activeCharacterAction.TurnProgress.IsDone())
            {
                activeCharacterAction.PerformAction(this, combat);
                activeCharacterAction.AfterAction();
                activeCharacterAction = null;
            }
        }
    }

    public void ApplyBuff(IBuff buff)
    {
        buffs.Add(buff);
        SingletonProvider.MainEventPublisher.Publish(EventType.BuffApplied, new BuffAppliedEvent(this, buff));
    }

}