using System.Collections.Generic;

public interface ICombatant
{
    void PerformAction(List<ICombatant> enemies);
    void BeAttacked(int attackStat);
    bool IsDead();
}
