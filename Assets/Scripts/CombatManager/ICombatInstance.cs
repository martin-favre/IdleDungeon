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

    public CombatInstanceFactory(ITimeProvider timeProvider, IPersistentDataStorage storage, IPlayerWallet wallet)
    {
        this.timeProvider = timeProvider;
        this.storage = storage;
        this.wallet = wallet;
    }

    public ICombatInstance CreateInstance(ICombatant[] playerChars, IEventRecipient<ICombatUpdateEvent> evRecipient)
    {
        int currentLevel = storage.GetInt(Constants.currentLevelKey, 1);
        return new CombatInstance(playerChars, new LevelGeneratedEnemyFactory(currentLevel), evRecipient, timeProvider, wallet);
    }
}

public interface ICombatInstance : IDisposable
{
    bool IsDone();
    void Update();

    ICombatReader CombatReader { get; }
}
