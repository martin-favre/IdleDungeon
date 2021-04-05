
using System;
using System.Collections.Generic;

public class CombatResults
{

}

public class CombatInstance : ICombatInstance, ICombatReader
{

    List<ICombatant> goodGuys;
    List<ICombatant> badGuys;

    public CombatInstance(List<ICombatant> playerChars, IEnemyFactory enemyFactory)
    {
        if (playerChars.Count < 1) throw new System.Exception("Starting combat without player");
        this.goodGuys = playerChars;
        badGuys = enemyFactory.GenerateEnemies();
    }
    public void Update()
    {
        if (IsDone()) return;
        foreach (var combatant in goodGuys)
        {
            combatant.PerformAction(badGuys);
            CleanOutDeadGuys(badGuys);
            CleanOutDeadGuys(goodGuys);
            if (IsDone()) return;
        }

        foreach (var combatant in badGuys)
        {
            combatant.PerformAction(goodGuys);
            CleanOutDeadGuys(badGuys);
            CleanOutDeadGuys(goodGuys);
            if (IsDone()) return;
        }

    }

    private void CleanOutDeadGuys(List<ICombatant> combatants)
    {
        combatants.RemoveAll((c) => c.IsDead());
    }

    public bool IsDone()
    {
        return badGuys.Count == 0 || goodGuys.Count == 0;
    }

    public void Dispose()
    {
        goodGuys.Clear();
        badGuys.Clear();
    }

    public CombatAttributes[] GetEnemiesAttributes()
    {
        CombatAttributes[] arr = new CombatAttributes[badGuys.Count];
        for (int i = 0; i < badGuys.Count; i++)
        {
            arr[i] = badGuys[i].Attributes;
        }
        return arr;
    }
}