
using System;
using System.Collections.Generic;

public class CombatResults
{

}

public class CombatInstance : ICombatInstance, ICombatReader
{

    List<ICombatant> goodGuys;
    Dictionary<ICombatant, float> combatProgress = new Dictionary<ICombatant, float>();
    List<ICombatant> badGuys;
    private readonly IEventRecipient<ICombatUpdateEvent> evRecipient;
    private readonly ITimeProvider timeProvider;

    public ICombatReader CombatReader => this;

    public CombatInstance(ICombatant[] playerChars,
    IEnemyFactory enemyFactory,
    IEventRecipient<ICombatUpdateEvent> evRecipient,
    ITimeProvider timeProvider)
    {
        if (playerChars.Length < 1) throw new System.Exception("Starting combat without player");
        this.goodGuys = new List<ICombatant>(playerChars);
        badGuys = enemyFactory.GenerateEnemies();
        this.evRecipient = evRecipient;
        this.timeProvider = timeProvider;
        goodGuys.ForEach(e => e.TurnProgress.ResetTurnProgress());
        badGuys.ForEach(e => e.TurnProgress.ResetTurnProgress());
    }
    public void Update()
    {
        if (IsDone()) return;
        foreach (var combatant in goodGuys)
        {
            if (ItsTheirTurn(combatant))
            {
                combatant.PerformAction(badGuys, this, evRecipient);
                CleanOutDeadGuys(badGuys);
                CleanOutDeadGuys(goodGuys);
                if (IsDone()) return;
            }
        }

        foreach (var combatant in badGuys)
        {
            if (ItsTheirTurn(combatant))
            {
                combatant.PerformAction(goodGuys, this, evRecipient);
                CleanOutDeadGuys(badGuys);
                CleanOutDeadGuys(goodGuys);
                if (IsDone()) return;
            }
        }
    }

    private bool ItsTheirTurn(ICombatant combatant)
    {
        if(combatant.TurnProgress == null) return true;
        return combatant.TurnProgress.IncrementTurnProgress(combatant.Attributes.Speed*timeProvider.DeltaTime);
    }

    private void CleanOutDeadGuys(List<ICombatant> combatants)
    {
        foreach (var item in combatants)
        {
            if (item.IsDead()) combatProgress.Remove(item);
        }
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

    public IDisposable Subscribe(IObserver<ICombatUpdateEvent> observer)
    {
        throw new NotImplementedException();
    }
}