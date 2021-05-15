using System;
using System.Collections.Generic;

public interface ICombatInstanceFactory
{
    ICombatInstance CreateInstance(ICombatant[] playerChars,
    IEventRecipient<ICombatUpdateEvent> evRecipient);
}

public class CombatInstanceFactory : ICombatInstanceFactory
{
    private readonly ITimeProvider timeProvider;
    private readonly IPersistentDataStorage storage;
    private readonly IPlayerWallet wallet;
    private readonly IRandomProvider randomProvider;

    public CombatInstanceFactory(ITimeProvider timeProvider, IPersistentDataStorage storage, IPlayerWallet wallet, IRandomProvider randomProvider)
    {
        this.timeProvider = timeProvider;
        this.storage = storage;
        this.wallet = wallet;
        this.randomProvider = randomProvider;
    }

    public ICombatInstance CreateInstance(ICombatant[] playerChars, IEventRecipient<ICombatUpdateEvent> evRecipient)
    {
        int currentLevel = storage.GetInt(Constants.currentLevelKey, 1);
        return new CombatInstance(playerChars, new LevelGeneratedEnemyFactory(currentLevel, randomProvider), evRecipient, timeProvider, wallet);
    }
}

public interface ICombatInstance : IDisposable
{
    public enum CombatResult
    {
        Unknown,
        PlayerWon,
        PlayerLost
    }
    CombatResult Result { get; }
    bool IsDone();
    void Update();

    ICombatReader CombatReader { get; }
}
