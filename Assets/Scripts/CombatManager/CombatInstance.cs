
using System.Collections.Generic;

public class CombatResults
{

}

public class CombatInstance : ICombatInstance
{

    float startTime;
    List<ICombatant> goodGuys;
    List<ICombatant> badGuys;

    public CombatInstance(List<ICombatant> playerChars)
    {
        startTime = UnityTime.Instance.Time;
        if (playerChars.Count < 1) throw new System.Exception("Starting combat without player");
        this.goodGuys = playerChars;
        badGuys = new List<ICombatant>();
        badGuys.Add(new SimpleCombatant());
    }
    public void Update()
    {
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
}