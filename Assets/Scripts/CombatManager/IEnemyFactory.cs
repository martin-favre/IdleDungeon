
using System.Collections.Generic;

public interface IEnemyFactory
{
    List<ICombatant> GenerateEnemies();
}

public class EnemyFactory : IEnemyFactory
{
    public List<ICombatant> GenerateEnemies()
    {
        var ret = new List<ICombatant>();
        ret.Add(new SimpleCombatant());
        ret.Add(new SimpleCombatant());
        ret.Add(new SimpleCombatant());
        ret.Add(new SimpleCombatant());
        return ret;
    }
}