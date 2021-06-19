using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : ICombatAttributes
{
    private readonly int playerIdentifier;
    private readonly WeakReference<ICharacter> owner;

    public double Attack { get => attack; }

    public double Speed => 50;

    private double attack;

    List<MultiplierUpgrade> attackUpgrades;
    List<KeyObserver<string, Upgrade>> attackUpgradeObservers;
    public PlayerAttributes(int playerIdentifier, WeakReference<ICharacter> owner)
    {
        this.playerIdentifier = playerIdentifier;
        this.owner = owner;
        InitializeAttackinessUpgrades();
        SetAttackiness();
    }

    private void InitializeAttackinessUpgrades()
    {
        attackUpgrades = new List<MultiplierUpgrade>()
        {
            new MultiplierUpgrade(500, 1, 50, 1.07f, GetAttackinessUpgradeKey(0, playerIdentifier)),
            new MultiplierUpgrade(50, 0, 500, 1.08f, GetAttackinessUpgradeKey(1, playerIdentifier)),
            new MultiplierUpgrade(500, 0, 5000, 1.09f, GetAttackinessUpgradeKey(2, playerIdentifier)),
        };

        attackUpgradeObservers = new List<KeyObserver<string, Upgrade>>(attackUpgrades.Count);
        attackUpgrades.ForEach(upgrade => attackUpgradeObservers.Add(new KeyObserver<string, Upgrade>(SingletonProvider.MainUpgradeManager, upgrade.StorageKey, e => SetAttackiness())));
    }

    public static string GetAttackinessUpgradeKey(int upgradeIndex, int player)
    {
        return "attackinesslevel" + upgradeIndex + "player" + player;
    }

    private void SetAttackiness()
    {
        attack = 0;
        attackUpgrades.ForEach(u => attack += ((MultiplierUpgrade)u).MultipliedValue);
        ICharacter chr;
        if (owner.TryGetTarget(out chr))
        {
            CentralEventHandler.Instance.Publish(EventType.CharacterAttributeChanged, new AttributeChangedEvent(chr));
        }

    }
}