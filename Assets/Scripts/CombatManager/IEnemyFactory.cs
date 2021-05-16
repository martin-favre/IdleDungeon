
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
        var ret = new List<ICharacter>();
        ret.Add(new LevelGeneratedCharacter(currentLevel));
        ret.Add(new LevelGeneratedCharacter(currentLevel));
        ret.Add(new LevelGeneratedCharacter(currentLevel));
        ret.Add(new LevelGeneratedCharacter(currentLevel));
        return ret;
    }
}