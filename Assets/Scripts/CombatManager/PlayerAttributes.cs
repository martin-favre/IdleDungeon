using System;
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

    SimpleObserver<Upgrade> attackinessLevel1Observer;
    SimpleObserver<Upgrade> attackinessLevel2Observer;
    SimpleObserver<Upgrade> attackinessLevel3Observer;
    SimpleObserver<Upgrade> healthinessLevel1Observer;
    SimpleObserver<Upgrade> healthinessLevel2Observer;
    SimpleObserver<Upgrade> healthinessLevel3Observer;
    Upgrade attackinessLevel1;
    Upgrade attackinessLevel2;
    Upgrade attackinessLevel3;
    Upgrade healthinessLevel1;
    Upgrade healthinessLevel2;
    Upgrade healthinessLevel3;

    public PlayerAttributes(IPersistentDataStorage storage, IEventRecipient<IPlayerCharacterUpdateEvent> recipient, IUpgradeManager upgradeManager)
    {
        this.storage = storage;
        this.recipient = recipient;
        {
            attackinessLevel1 = UpgradeManager.Instance.GetUpgrade(UpgradeType.AttackinessLevel1);
            attackinessLevel1Observer = new SimpleObserver<Upgrade>(attackinessLevel1, (u) => SetAttackiness(u, attackinessLevel2, attackinessLevel3));
            attackinessLevel2 = UpgradeManager.Instance.GetUpgrade(UpgradeType.AttackinessLevel2);
            attackinessLevel2Observer = new SimpleObserver<Upgrade>(attackinessLevel2, (u) => SetAttackiness(attackinessLevel1, u, attackinessLevel3));
            attackinessLevel3 = UpgradeManager.Instance.GetUpgrade(UpgradeType.HealthinessLevel1);
            attackinessLevel3Observer = new SimpleObserver<Upgrade>(attackinessLevel3, (u) => SetAttackiness(attackinessLevel1, attackinessLevel2, u));

            healthinessLevel1 = UpgradeManager.Instance.GetUpgrade(UpgradeType.HealthinessLevel1);
            healthinessLevel1Observer = new SimpleObserver<Upgrade>(healthinessLevel1, (u) => SetMaxHp(healthinessLevel1, healthinessLevel2, healthinessLevel3));
            healthinessLevel2 = UpgradeManager.Instance.GetUpgrade(UpgradeType.HealthinessLevel2);
            healthinessLevel2Observer = new SimpleObserver<Upgrade>(healthinessLevel2, (u) => SetMaxHp(healthinessLevel1, healthinessLevel2, healthinessLevel3));
            healthinessLevel3 = UpgradeManager.Instance.GetUpgrade(UpgradeType.HealthinessLevel3);
            healthinessLevel3Observer = new SimpleObserver<Upgrade>(healthinessLevel3, (u) => SetMaxHp(healthinessLevel1, healthinessLevel2, healthinessLevel3));
        }
        currentHp = maxHp; // todo, store currenthp in DB
        SetAttackiness(attackinessLevel1, attackinessLevel2, attackinessLevel3);
        SetMaxHp(healthinessLevel1, healthinessLevel2, healthinessLevel3);
    }

    private void SetAttackiness(Upgrade attackinessLevel1, Upgrade attackinessLevel2, Upgrade attackinessLevel3)
    {
        attack = attackinessLevel1.Level * 5 + attackinessLevel2.Level * 1000 + attackinessLevel3.Level * 10000;
    }

    void SetMaxHp(Upgrade healthinessLevel1, Upgrade healthinessLevel2, Upgrade healthinessLevel3)
    {
        double oldMaxHp = maxHp;
        double oldCurrentHpPart = currentHp / oldMaxHp;
        maxHp = healthinessLevel1.Level * 50 + healthinessLevel2.Level * 1000 + healthinessLevel3.Level * 10000;
        currentHp = oldCurrentHpPart * maxHp;
        recipient.RecieveEvent(new PlayerCharacterAttributeUpdateEvent(this));
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

    public void Damage(double damage)
    {
        currentHp -= damage;
        if (currentHp <= 0) currentHp = 0;
    }

    public void Heal(double healing)
    {
        currentHp += healing;
        if (currentHp > maxHp) currentHp = maxHp;
    }


    public bool IsDead()
    {
        return currentHp <= 0;
    }
}