
using System.Collections.Generic;

public interface IEnemyFactory
{
    List<ICharacter> GenerateEnemies();
}

public class LevelGeneratedEnemyFactory : IEnemyFactory
{
    private readonly int currentLevel;

    public LevelGeneratedEnemyFactory(int currentLevel)
    {
        this.currentLevel = currentLevel;
    }
    public List<ICharacter> GenerateEnemies()
    {
        int nofEnemies = SingletonProvider.MainRandomProvider.RandomInt(1, 5);
        var ret = new List<ICharacter>();
        // Make enemies less powerful if more are out
        // But should still be slightly more powerful than one to make it more spicy
        float powerFactor = 1 / (nofEnemies * 0.8f);
        for (int i = 0; i < nofEnemies; i++)
        {
            ret.Add(new LevelGeneratedCharacter(EnemyTemplates.GetRandom(), currentLevel, powerFactor));
        }
        return ret;
    }
}