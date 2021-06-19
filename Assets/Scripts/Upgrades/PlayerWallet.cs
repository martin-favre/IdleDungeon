/*
    Contains, stores and loads the player's currencies
*/

using System;
using System.Collections.Generic;
using Logging;

public interface IPlayerWalletUpdateEvent
{
    IPlayerWallet Wallet { get; }
}

public class PlayerWalletUpdateEvent : IPlayerWalletUpdateEvent
{
    private readonly IPlayerWallet wallet;

    public PlayerWalletUpdateEvent(IPlayerWallet wallet)
    {
        this.wallet = wallet;
    }

    public IPlayerWallet Wallet => wallet;

}

public class ExperienceGainedEvent : PlayerWalletUpdateEvent
{
    public ExperienceGainedEvent(IPlayerWallet wallet) : base(wallet)
    {
    }
}

public class ExperienceLostEvent : PlayerWalletUpdateEvent
{
    public ExperienceLostEvent(IPlayerWallet wallet) : base(wallet)
    {
    }
}

public class GoldLostEvent : PlayerWalletUpdateEvent
{
    public GoldLostEvent(IPlayerWallet wallet) : base(wallet)
    {
    }
}
public class GoldGainedEvent : PlayerWalletUpdateEvent
{
    public GoldGainedEvent(IPlayerWallet wallet) : base(wallet)
    {
    }
}

public interface IPlayerWallet : IObservable<IPlayerWalletUpdateEvent>
{
    double Experience { get; }
    double Gold { get; }

    void AddExperience(double amount);
    void RemoveExperience(double amount);
    void AddGold(double amount);
    void RemoveGold(double amount);
}

public class PlayerWallet : IPlayerWallet
{
    private static PlayerWallet instance;
    public static PlayerWallet Instance { get => instance; }
    private static readonly LilLogger logger = new LilLogger(typeof(PlayerWallet).ToString());
    private readonly IPersistentDataStorage storage;

    private double experience = 0;
    public double Experience => experience;
    private double gold = 0;
    public double Gold => gold;

    List<IObserver<IPlayerWalletUpdateEvent>> observers = new List<IObserver<IPlayerWalletUpdateEvent>>();

    IObserver<IPersistentStorageUpdateEvent> dbObserver;

    const int defaultValue = 0;

    static PlayerWallet()
    {
        instance = new PlayerWallet(SingletonProvider.MainDataStorage);
    }

    public PlayerWallet(IPersistentDataStorage storage)
    {
        experience = storage.GetFloat(Constants.experienceKey, defaultValue);
        gold = storage.GetFloat(Constants.goldKey, defaultValue);
        this.storage = storage;
        dbObserver = new KeyObserver<string, IPersistentStorageUpdateEvent>(storage, Constants.experienceKey, OnDBUpdate);
    }

    private void OnDBUpdate(IPersistentStorageUpdateEvent e)
    {
        if (e is DataClearedUpdateEvent ce)
        {
            experience = defaultValue;
            gold = defaultValue;
            SendEvent(new ExperienceLostEvent(this));
            SendEvent(new GoldLostEvent(this));
        }
    }

    public void AddExperience(double amount)
    {
        experience += amount;
        storage.SetFloat(Constants.experienceKey, (float)experience);
        SendEvent(new ExperienceGainedEvent(this));
    }

    public void RemoveExperience(double amount)
    {
        experience -= amount;
        storage.SetFloat(Constants.experienceKey, (float)experience);
        if (experience < 0)
        {
            experience = 0;
            logger.Log("Going to negative experience", LogLevel.Warning);
        }
        SendEvent(new ExperienceLostEvent(this));

    }

    private void SendEvent(IPlayerWalletUpdateEvent update)
    {
        observers.ForEach(e => e.OnNext(update));
    }

    public IDisposable Subscribe(IObserver<IPlayerWalletUpdateEvent> observer)
    {
        return new SimpleUnsubscriber<IPlayerWalletUpdateEvent>(observers, observer);
    }

    public void AddGold(double amount)
    {
        gold += amount;
        storage.SetFloat(Constants.goldKey, (float)gold);
        SendEvent(new GoldGainedEvent(this));
    }

    public void RemoveGold(double amount)
    {
        gold -= amount;
        storage.SetFloat(Constants.goldKey, (float)gold);
        if (gold < 0)
        {
            gold = 0;
            logger.Log("Going to negative gold", LogLevel.Warning);
        }
        SendEvent(new GoldLostEvent(this));
    }
}