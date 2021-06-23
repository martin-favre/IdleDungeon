
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
        return EnemyTemplates.GetRandomEncounter();
    }
}