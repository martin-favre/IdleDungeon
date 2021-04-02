using System.Collections.Generic;

class SimpleCombatant : ICombatant
{
    public SimpleCombatant()
    {
    }

    public void BeAttacked(int AttackStat)
    {
        throw new System.NotImplementedException();
    }

    public void PerformAction(List<ICombatant> enemies)
    {
        if(enemies.Count == 0) return;
        ICombatant target = enemies[0];
        target.BeAttacked(1);
    }
}
