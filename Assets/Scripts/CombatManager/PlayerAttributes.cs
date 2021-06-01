using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : ICombatAttributes, IDisposable
{
    private readonly int playerIdentifier;
    private ICharacter owner;

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
    IObserver<IPersistentStorageUpdateEvent> storageObserver;
    public PlayerAttributes(int playerIdentifier, ICharacter owner)
    {
        this.playerIdentifier = playerIdentifier;
        this.owner = owner;
        InitializeAttackinessUpgrades();
        SetAttackiness();
        InitializeHealthinessUpgrades();
        currentHp = SingletonProvider.MainDataStorage.GetFloat(GetCurrentHpKey(playerIdentifier), (float)maxHp);
        storageObserver = new KeyObserver<string, IPersistentStorageUpdateEvent>(SingletonProvider.MainDataStorage, GetCurrentHpKey(playerIdentifier), e =>
        {
            if (e is DataClearedUpdateEvent clr)
            {
                var oldHp = currentHp;
                currentHp = maxHp;
                if (currentHp != oldHp) CharacterEventPublisher.Instance.Publish(CharacterUpdateEventType.CurrentHpChanged, new CurrentHpChanged(currentHp - oldHp, owner));
            }
        });
        SetMaxHp();
    }

    private void InitializeAttackinessUpgrades()
    {
        attackUpgrades = new List<MultiplierUpgrade>()
        {
            new MultiplierUpgrade(5, 1, 50, 1.07f, GetAttackinessUpgradeKey(0, playerIdentifier)),
            new MultiplierUpgrade(50, 0, 1000, 1.09f, GetAttackinessUpgradeKey(1, playerIdentifier)),
            new MultiplierUpgrade(500, 0, 10000, 1.11f, GetAttackinessUpgradeKey(2, playerIdentifier)),
        };

        attackUpgradeObservers = new List<KeyObserver<string, Upgrade>>(attackUpgrades.Count);
        attackUpgrades.ForEach(upgrade => attackUpgradeObservers.Add(new KeyObserver<string, Upgrade>(SingletonProvider.MainUpgradeManager, upgrade.StorageKey, e => SetAttackiness())));
    }

    private void InitializeHealthinessUpgrades()
    {
        healthUpgrades = new List<MultiplierUpgrade>() {
            new MultiplierUpgrade(50, 1, 50, 1.07f, GetHealthinessUpgradeKey(0, playerIdentifier)),
            new MultiplierUpgrade(500, 0, 1000, 1.09f, GetHealthinessUpgradeKey(1, playerIdentifier)),
            new MultiplierUpgrade(5000, 0, 10000, 1.11f, GetHealthinessUpgradeKey(2, playerIdentifier)),
        };
        healthUpgradeObservers = new List<KeyObserver<string, Upgrade>>(healthUpgrades.Count);
        healthUpgrades.ForEach(upgrade => healthUpgradeObservers.Add(new KeyObserver<string, Upgrade>(SingletonProvider.MainUpgradeManager, upgrade.StorageKey, e => SetMaxHp())));

    }

    public static string GetCurrentHpKey(int player)
    {
        return "currenthpplayer" + player;
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
        CharacterEventPublisher.Instance.Publish(CharacterUpdateEventType.AttributeChanged, new AttributeChangedEvent(owner));
    }

    void SetMaxHp()
    {
        double oldMaxHp = maxHp;
        double oldCurrentHpPart = currentHp / oldMaxHp;
        maxHp = 0;
        healthUpgrades.ForEach(u => maxHp += ((MultiplierUpgrade)u).MultipliedValue);
        currentHp = oldCurrentHpPart * maxHp;
        CharacterEventPublisher.Instance.Publish(CharacterUpdateEventType.AttributeChanged, new AttributeChangedEvent(owner));
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
        CharacterEventPublisher.Instance.Publish(CharacterUpdateEventType.AttributeChanged, new AttributeChangedEvent(owner));
    }

    public void Damage(double damage)
    {
        var oldCurrentHp = currentHp;
        currentHp -= damage;
        if (currentHp <= 0) currentHp = 0;
        if (currentHp != oldCurrentHp)
        {
            SingletonProvider.MainDataStorage.SetFloat(GetCurrentHpKey(playerIdentifier), (float)currentHp);
            CharacterEventPublisher.Instance.Publish(CharacterUpdateEventType.CurrentHpChanged, new CurrentHpChanged(currentHp-oldCurrentHp, owner));
        }
    }

    public void Heal(double healing)
    {
        var oldCurrentHp = currentHp;
        currentHp += healing;
        if (currentHp > maxHp) currentHp = maxHp;
        if (currentHp != oldCurrentHp)
        {
            SingletonProvider.MainDataStorage.SetFloat(GetCurrentHpKey(playerIdentifier), (float)currentHp);
            CharacterEventPublisher.Instance.Publish(CharacterUpdateEventType.CurrentHpChanged, new CurrentHpChanged(currentHp-oldCurrentHp, owner));
        }
    }


    public bool IsDead()
    {
        return currentHp <= 0;
    }

    public void Dispose()
    {
        owner = null;
    }
}