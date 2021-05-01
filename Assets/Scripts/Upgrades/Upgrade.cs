using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class Upgrade : IObservable<Upgrade>
{
    private int level;
    private readonly float baseCost;
    private readonly float costMultiplier;
    private readonly IPersistentDataStorage storage;

    private List<IObserver<Upgrade>> observers = new List<IObserver<Upgrade>>();

    public float Cost { get => baseCost * Mathf.Pow(costMultiplier, level); }
    public int Level { get => level; }
    public abstract string StorageKey { get; }

    public Upgrade(int initialLevel, float baseCost, float costMultiplier, IPersistentDataStorage storage)
    {
        level = storage.GetInt(StorageKey, initialLevel);
        this.baseCost = baseCost;
        this.costMultiplier = costMultiplier;
        this.storage = storage;
    }
    public void LevelUp()
    {
        level++;
        storage.SetInt(StorageKey, level);
        NotifyObservers();
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