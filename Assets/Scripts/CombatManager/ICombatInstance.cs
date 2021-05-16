using System;
using System.Collections.Generic;

public interface ICombatInstanceFactory
{
    ICombatInstance CreateInstance(ICharacter[] playerChars,
    IEventRecipient<ICombatUpdateEvent> evRecipient);
}

public class CombatInstanceFactory : ICombatInstanceFactory
{
    public CombatInstanceFactory()
    {
    }

    public ICombatInstance CreateInstance(ICharacter[] playerChars, IEventRecipient<ICombatUpdateEvent> evRecipient)
    {
        int currentLevel = SingletonProvider.MainDataStorage.GetInt(Constants.currentLevelKey, 1);
        return new CombatInstance(playerChars, new LevelGeneratedEnemyFactory(currentLevel), evRecipient);
    }
}

public interface ICombatInstance : IDisposable
{
    enum CombatResult
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
