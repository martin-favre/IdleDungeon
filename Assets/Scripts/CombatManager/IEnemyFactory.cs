
using System.Collections.Generic;

public interface IEnemyFactory
{
    List<ICombatant> GenerateEnemies();
}

public class LevelGeneratedEnemyFactory : IEnemyFactory
{
    private readonly int currentLevel;
    private readonly IRandomProvider randomProvider;

    public LevelGeneratedEnemyFactory(int currentLevel, IRandomProvider randomProvider)
    {
        this.currentLevel = currentLevel;
        this.randomProvider = randomProvider;
    }
    public List<ICombatant> GenerateEnemies()
    {
        var ret = new List<ICombatant>();
        ret.Add(new LevelGeneratedCombatant(currentLevel, randomProvider));
        ret.Add(new LevelGeneratedCombatant(currentLevel, randomProvider));
        ret.Add(new LevelGeneratedCombatant(currentLevel, randomProvider));
        ret.Add(new LevelGeneratedCombatant(currentLevel, randomProvider));
        return ret;
    }
}