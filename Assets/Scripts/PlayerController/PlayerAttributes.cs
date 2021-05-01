using UnityEngine;

public class PlayerAttributes : ICombatAttributes
{
    private readonly IPersistentDataStorage storage;
    private readonly IEventRecipient<IPlayerCharacterUpdateEvent> recipient;

    public double MaxHp { get => maxHp; }
    
    public double CurrentHp { get => currentHp; }

    public double Attack { get => attack; }

    public double Speed => 100;

    private double attack;

    private double maxHp = 100;
    private double currentHp;

    SimpleObserver<Upgrade> attackinessObserver;
    SimpleObserver<Upgrade> healthinessObserver;

    public PlayerAttributes(IPersistentDataStorage storage, IEventRecipient<IPlayerCharacterUpdateEvent> recipient)
    {
        this.storage = storage;
        this.recipient = recipient;
        attackinessObserver = new SimpleObserver<Upgrade>(UpgradeManager.Instance.Attackiness, (u) => SetAttack(u.Level));
        SetAttack(UpgradeManager.Instance.Attackiness.Level);
        healthinessObserver = new SimpleObserver<Upgrade>(UpgradeManager.Instance.Healthiness, (u) => SetMaxHp(u.Level));
        SetMaxHp(UpgradeManager.Instance.Healthiness.Level);
        currentHp = maxHp; // todo, store currenthp in DB
    }


    void SetMaxHp(int level)
    {
        double oldMaxHp = maxHp;
        double oldCurrentHpPart = currentHp / oldMaxHp;
        maxHp = 50 + level * 50;
        // If maxhp goes up, correct the currenthp to be percentually the same as before
        if (maxHp > oldMaxHp)
        {
            currentHp = oldCurrentHpPart * maxHp;
        }
        recipient.RecieveEvent(new PlayerCharacterAttributeUpdateEvent(this));
    }
    
    void SetAttack(int level)
    {
        attack = 2 + level * 2;
    }

    public void Damage(double damage)
    {
        currentHp -= damage;
        if (currentHp <= 0) currentHp = 0;
    }

    public bool IsDead()
    {
        return false; // Temporary during development to avoid dying all the time
    }
}