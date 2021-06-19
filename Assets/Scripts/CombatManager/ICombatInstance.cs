using System;
using System.Collections.Generic;

public interface ICombatInstanceFactory
{
    ICombatInstance CreateInstance(ICharacter[] playerChars);
}

public class CombatInstanceFactory : ICombatInstanceFactory
{
    public CombatInstanceFactory()
    {
    }

    public ICombatInstance CreateInstance(ICharacter[] playerChars)
    {
        int currentLevel = SingletonProvider.MainDataStorage.GetInt(Constants.currentLevelKey, 1);
        return new CombatInstance(playerChars, new LevelGeneratedEnemyFactory(currentLevel));
    }
}

public interface ICombatInstance : IDisposable
{
    // returns null if combat is not done
    // returns the result of the combat if done
    CombatResult Update();

    ICombatReader CombatReader { get; }
}
