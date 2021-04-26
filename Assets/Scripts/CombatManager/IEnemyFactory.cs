
using System.Collections.Generic;

public interface IEnemyFactory
{
    List<ICombatant> GenerateEnemies();
}

public class LevelGeneratedEnemyFactory : IEnemyFactory
{
    private readonly int currentLevel;

    public LevelGeneratedEnemyFactory(int currentLevel)
    {
        this.currentLevel = currentLevel;
    }
    public List<ICombatant> GenerateEnemies()
    {
        var ret = new List<ICombatant>();
        ret.Add(new LevelGeneratedCombatant(currentLevel));
        ret.Add(new LevelGeneratedCombatant(currentLevel));
        ret.Add(new LevelGeneratedCombatant(currentLevel));
        ret.Add(new LevelGeneratedCombatant(currentLevel));
        return ret;
    }
}