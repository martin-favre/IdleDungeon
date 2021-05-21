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

public class ExperienceGainedEvent : IPlayerWalletUpdateEvent
{
    private readonly IPlayerWallet wallet;

    public ExperienceGainedEvent(IPlayerWallet wallet)
    {
        this.wallet = wallet;
    }

    public IPlayerWallet Wallet => wallet;
}

public class ExperienceLostEvent : IPlayerWalletUpdateEvent
{
    private readonly IPlayerWallet wallet;

    public ExperienceLostEvent(IPlayerWallet wallet)
    {
        this.wallet = wallet;
    }

    public IPlayerWallet Wallet => wallet;
}

public interface IPlayerWallet : IObservable<IPlayerWalletUpdateEvent>
{
    double Experience { get; }
    void AddExperience(double amount);
    void RemoveExperience(double amount);
}

public class PlayerWallet : IPlayerWallet
{
    private static PlayerWallet instance;
    public static PlayerWallet Instance { get => instance; }
    private static readonly LilLogger logger = new LilLogger(typeof(PlayerWallet).ToString());
    private readonly IPersistentDataStorage storage;

    private double experience = 0;
    public double Experience => experience;

    List<IObserver<IPlayerWalletUpdateEvent>> observers = new List<IObserver<IPlayerWalletUpdateEvent>>();

    IObserver<IPersistentStorageUpdateEvent> dbObserver;

    const int defaultValue = 0;

    static PlayerWallet()
    {
        instance = new PlayerWallet(PlayerPrefsReader.Instance);
    }

    public PlayerWallet(IPersistentDataStorage storage)
    {
        experience = storage.GetFloat(Constants.experienceKey, defaultValue);
        this.storage = storage;
        dbObserver = new KeyObserver<string, IPersistentStorageUpdateEvent>(storage, Constants.experienceKey, OnDBUpdate);
    }

    private void OnDBUpdate(IPersistentStorageUpdateEvent e)
    {
        if (e is DataClearedUpdateEvent ce)
        {
            experience = defaultValue;
            SendEvent(new ExperienceLostEvent(this));
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
}