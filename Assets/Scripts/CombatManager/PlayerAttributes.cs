using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : ICombatAttributes
{
    private readonly IPersistentDataStorage storage;
    private readonly IEventRecipient<ICharacterUpdateEvent> recipient;

    public double MaxHp { get => maxHp; }

    public double CurrentHp { get => currentHp; }

    public double Attack { get => attack; }

    public double Speed => 100;

    private double attack;

    private double maxHp = 100;
    private double currentHp;

    List<MultiplierUpgrade> attackUpgrades;
    List<MultiplierUpgrade> healthUpgrades;
    List<KeyObserver<string, Upgrade>> attackUpgradeObservers;
    List<KeyObserver<string, Upgrade>> healthUpgradeObservers;
    public PlayerAttributes(IPersistentDataStorage storage, IEventRecipient<ICharacterUpdateEvent> recipient, IUpgradeManager upgradeManager, int playerIdentifier)
    {
        this.storage = storage;
        this.recipient = recipient;

        attackUpgrades = new List<MultiplierUpgrade>()
        {
            new MultiplierUpgrade(5, 1, 50, 1.07f, GetAttackinessUpgradeKey(0, playerIdentifier), storage, PlayerWallet.Instance, upgradeManager),
            new MultiplierUpgrade(50, 0, 1000, 1.09f, GetAttackinessUpgradeKey(1, playerIdentifier), storage, PlayerWallet.Instance, upgradeManager),
            new MultiplierUpgrade(500, 0, 10000, 1.11f, GetAttackinessUpgradeKey(2, playerIdentifier), storage, PlayerWallet.Instance, upgradeManager),
        };

        attackUpgradeObservers = new List<KeyObserver<string, Upgrade>>(attackUpgrades.Count);
        attackUpgrades.ForEach(upgrade => attackUpgradeObservers.Add(new KeyObserver<string, Upgrade>(UpgradeManager.Instance, upgrade.StorageKey, e => SetAttackiness())));
        SetAttackiness();
        healthUpgrades = new List<MultiplierUpgrade>() {
            new MultiplierUpgrade(50, 1, 50, 1.07f, GetHealthinessUpgradeKey(0, playerIdentifier), storage, PlayerWallet.Instance, upgradeManager),
            new MultiplierUpgrade(500, 0, 1000, 1.09f, GetHealthinessUpgradeKey(1, playerIdentifier), storage, PlayerWallet.Instance, upgradeManager),
            new MultiplierUpgrade(5000, 0, 10000, 1.11f, GetHealthinessUpgradeKey(2, playerIdentifier), storage, PlayerWallet.Instance, upgradeManager),
        };
        healthUpgradeObservers = new List<KeyObserver<string, Upgrade>>(attackUpgrades.Count);
        healthUpgrades.ForEach(upgrade => attackUpgradeObservers.Add(new KeyObserver<string, Upgrade>(UpgradeManager.Instance, upgrade.StorageKey, e => SetMaxHp())));
        currentHp = maxHp; // todo, store currenthp in DB
        SetMaxHp();
    }

    public static string GetAttackinessUpgradeKey(int upgradeIndex, int player)
    {
        return "attackinesslevel" + upgradeIndex + "player" + player;
    }
    public static string GetHealthinessUpgradeKey(int upgradeIndex, int player)
    {
        return "healthinesslevel" + upgradeIndex + "player" + player;
    }


    private void SetAttackiness()
    {
        attack = 0;
        attackUpgrades.ForEach(u => attack += ((MultiplierUpgrade)u).MultipliedValue);
        recipient.RecieveEvent(new AttributeChangedEvent(this));
    }

    void SetMaxHp()
    {
        double oldMaxHp = maxHp;
        double oldCurrentHpPart = currentHp / oldMaxHp;
        maxHp = 0;
        healthUpgrades.ForEach(u => maxHp += ((MultiplierUpgrade)u).MultipliedValue);
        currentHp = oldCurrentHpPart * maxHp;
        recipient.RecieveEvent(new AttributeChangedEvent(this));
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
        recipient.RecieveEvent(new AttributeChangedEvent(this));
    }

    public void Damage(double damage)
    {
        currentHp -= damage;
        if (currentHp <= 0) currentHp = 0;
        recipient.RecieveEvent(new AttributeChangedEvent(this));
    }

    public void Heal(double healing)
    {
        currentHp += healing;
        if (currentHp > maxHp) currentHp = maxHp;
        recipient.RecieveEvent(new AttributeChangedEvent(this));
    }


    public bool IsDead()
    {
        return currentHp <= 0;
    }
}