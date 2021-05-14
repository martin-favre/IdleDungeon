using System;
using System.Collections.Generic;
using UnityEngine;
public class Upgrade
{
    private int level;
    private readonly float baseCost;
    private readonly float costMultiplier;
    private readonly string storageKey;
    private readonly UpgradeType upgradeType;
    private readonly IPersistentDataStorage storage;
    private readonly IPlayerWallet wallet;
    public float Cost { get => baseCost * Mathf.Pow(costMultiplier, level); }
    public int Level { get => level; }

    public string StorageKey { get => storageKey; }

    public Upgrade(
        int initialLevel,
        float baseCost,
        float costMultiplier,
        string storageKey,
        IPersistentDataStorage storage,
        IPlayerWallet wallet,
        IUpgradeManager upgradeManager)
    {
        level = storage.GetInt(StorageKey, initialLevel);
        this.baseCost = baseCost;
        this.costMultiplier = costMultiplier;
        this.storageKey = storageKey;
        this.storage = storage;
        this.wallet = wallet;
        upgradeManager.SetUpgrade(StorageKey, this);
    }
    public void LevelUp()
    {
        if (Cost <= wallet.Experience)
        {
            wallet.RemoveExperience(Cost);
            level++;
            storage.SetInt(StorageKey, level);
        }
    }


}