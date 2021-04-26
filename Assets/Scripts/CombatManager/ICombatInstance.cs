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

    public CombatInstanceFactory(ITimeProvider timeProvider, IPersistentDataStorage storage)
    {
        this.timeProvider = timeProvider;
        this.storage = storage;
    }

    public ICombatInstance CreateInstance(ICombatant[] playerChars, IEventRecipient<ICombatUpdateEvent> evRecipient)
    {
        int currentLevel = storage.GetInt(Constants.currentLevelKey, 1);
        return new CombatInstance(playerChars, new LevelGeneratedEnemyFactory(currentLevel), evRecipient, timeProvider);
    }
}

public interface ICombatInstance : IDisposable
{
    bool IsDone();
    void Update();

    ICombatReader CombatReader { get; }
}
