using System;
using System.Collections.Generic;
using UnityEngine;
public class Upgrade
{
    private int level;
    private readonly int initialLevel;
    private readonly float baseCost;
    private readonly float costMultiplier;
    private readonly string storageKey;
    private readonly UpgradeType upgradeType;
    public float Cost { get => baseCost * Mathf.Pow(costMultiplier, level); }
    public int Level { get => level; }
    public string StorageKey { get => storageKey; }

    KeyObserver<string, IPersistentStorageUpdateEvent> observer;

    public Upgrade(
        int initialLevel,
        float baseCost,
        float costMultiplier,
        string storageKey)
    {
        level = SingletonProvider.MainDataStorage.GetInt(StorageKey, initialLevel);
        this.initialLevel = initialLevel;
        this.baseCost = baseCost;
        this.costMultiplier = costMultiplier;
        this.storageKey = storageKey;
        SingletonProvider.MainUpgradeManager.SetUpgrade(StorageKey, this);
        observer = new KeyObserver<string, IPersistentStorageUpdateEvent>(SingletonProvider.MainDataStorage, storageKey, e =>
        {
            int oldLevel = level;
            if (e is IntPersistentStorageUpdateEvent ev) level = ev.Value;
            if (e is DataClearedUpdateEvent) level = this.initialLevel;
            if (level != oldLevel)
            {
                SingletonProvider.MainUpgradeManager.RecieveEvent(StorageKey);
            }
        });
    }

    public void LevelUp()
    {
        if (Cost <= SingletonProvider.MainPlayerWallet.Experience)
        {
            SingletonProvider.MainPlayerWallet.RemoveExperience(Cost);
            level++;
            SingletonProvider.MainDataStorage.SetInt(StorageKey, level);
        }
    }


}