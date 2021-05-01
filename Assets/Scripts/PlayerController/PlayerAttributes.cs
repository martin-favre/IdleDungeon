using UnityEngine;

public class PlayerAttributes : ICombatAttributes
{
    private readonly IPersistentDataStorage storage;
    private readonly IEventRecipient<IPlayerCharacterUpdateEvent> recipient;

    public int MaxHp { get => maxHp; }
    
    public int CurrentHp { get => currentHp; }

    public int Attack { get => attack; }

    public int Speed => 100;

    private int attack;

    private int maxHp = 100;
    private int currentHp;

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
        int oldMaxHp = maxHp;
        float oldCurrentHpPart = ((float)currentHp) / ((float)oldMaxHp);
        maxHp = 50 + level * 50;
        // If maxhp goes up, correct the currenthp to be percentually the same as before
        if (maxHp > oldMaxHp)
        {
            currentHp = Mathf.RoundToInt(oldCurrentHpPart * maxHp);
        }
        recipient.RecieveEvent(new PlayerCharacterAttributeUpdateEvent(this));
    }
    
    void SetAttack(int level)
    {
        attack = 2 + level * 2;
    }

    public void Damage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0) currentHp = 0;
    }

    public bool IsDead()
    {
        return false; // Temporary during development to avoid dying all the time
    }
}