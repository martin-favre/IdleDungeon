using System;
using System.Collections.Generic;
using UnityEngine;
public class Upgrade : IObservable<Upgrade>
{
    private int level;
    private readonly float baseCost;
    private readonly float costMultiplier;
    private readonly string storageKey;
    private readonly IPersistentDataStorage storage;
    private readonly IPlayerWallet wallet;
    private List<IObserver<Upgrade>> observers = new List<IObserver<Upgrade>>();

    public float Cost { get => baseCost * Mathf.Pow(costMultiplier, level); }
    public int Level { get => level; }

    public Upgrade(int initialLevel, float baseCost, float costMultiplier, string storageKey, IPersistentDataStorage storage, IPlayerWallet wallet)
    {
        level = storage.GetInt(storageKey, initialLevel);
        this.baseCost = baseCost;
        this.costMultiplier = costMultiplier;
        this.storageKey = storageKey;
        this.storage = storage;
        this.wallet = wallet;
    }
    public void LevelUp()
    {
        if (Cost <= wallet.Experience)
        {
            wallet.RemoveExperience(Cost);
            level++;
            storage.SetInt(storageKey, level);
            NotifyObservers();
        }
    }

    void NotifyObservers()
    {
        foreach (var item in observers)
        {
            item.OnNext(this);
        }
    }

    public IDisposable Subscribe(IObserver<Upgrade> observer)
    {
        return new SimpleUnsubscriber<Upgrade>(observers, observer);
    }
}