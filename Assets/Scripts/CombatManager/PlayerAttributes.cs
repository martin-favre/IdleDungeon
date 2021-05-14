using System;
using System.Collections.Generic;
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

    List<MultiplierUpgrade> attackUpgrades;
    public PlayerAttributes(IPersistentDataStorage storage, IEventRecipient<IPlayerCharacterUpdateEvent> recipient, IUpgradeManager upgradeManager, int playerIdentifier)
    {
        this.storage = storage;
        this.recipient = recipient;

        attackUpgrades = new List<MultiplierUpgrade>()
        {
            new MultiplierUpgrade(5, 1, 50, 1.07f, GetAttackinessUpgradeKey(0, playerIdentifier), storage, PlayerWallet.Instance, upgradeManager),
            new MultiplierUpgrade(50, 0, 1000, 1.09f, GetAttackinessUpgradeKey(1, playerIdentifier), storage, PlayerWallet.Instance, upgradeManager),
            new MultiplierUpgrade(500, 0, 10000, 1.11f, GetAttackinessUpgradeKey(2, playerIdentifier), storage, PlayerWallet.Instance, upgradeManager),
            new MultiplierUpgrade(5, 1, 50, 1.07f, GetHealthinessUpgradeKey(0, playerIdentifier), storage, PlayerWallet.Instance, upgradeManager),
            new MultiplierUpgrade(50, 0, 1000, 1.09f, GetHealthinessUpgradeKey(1, playerIdentifier), storage, PlayerWallet.Instance, upgradeManager),
            new MultiplierUpgrade(500, 0, 10000, 1.11f, GetHealthinessUpgradeKey(2, playerIdentifier), storage, PlayerWallet.Instance, upgradeManager),
        };



        currentHp = maxHp; // todo, store currenthp in DB
    }

    public static string GetAttackinessUpgradeKey(int upgradeIndex, int player)
    {
        return "attackinesslevel" + upgradeIndex + "player" + player;
    }
    public static string GetHealthinessUpgradeKey(int upgradeIndex, int player)
    {
        return "healthinesslevel" + upgradeIndex + "player" + player;
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