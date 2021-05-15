
using System;
using System.Collections.Generic;

public class CombatResults
{

}

public class CombatInstance : ICombatInstance, ICombatReader
{

    List<ICombatant> goodGuys;
    List<ICombatant> badGuys;
    private readonly IEventRecipient<ICombatUpdateEvent> evRecipient;

    public ICombatReader CombatReader => this;

    public ICombatInstance.CombatResult Result
    {
        get
        {
            if (!IsDone()) return ICombatInstance.CombatResult.Unknown;
            return goodGuys.Count > 0 ? ICombatInstance.CombatResult.PlayerWon : ICombatInstance.CombatResult.PlayerLost;
        }
    }
    public CombatInstance(ICombatant[] playerChars,
    IEnemyFactory enemyFactory,
    IEventRecipient<ICombatUpdateEvent> evRecipient)
    {
        this.goodGuys = new List<ICombatant>(playerChars);
        badGuys = enemyFactory.GenerateEnemies();
        this.evRecipient = evRecipient;
        goodGuys.ForEach(e => e.TurnProgress.ResetTurnProgress());
        badGuys.ForEach(e => e.TurnProgress.ResetTurnProgress());
    }

    public void Update()
    {
        if (IsDone()) return;
        {
            ICombatant[] goodGuysCopy = goodGuys.ToArray(); // to avoid modifying the original while iterating
            foreach (var combatant in goodGuysCopy)
            {
                if (ItsTheirTurn(combatant) && !combatant.IsDead())
                {
                    combatant.PerformAction(badGuys, this, evRecipient);
                    GenerateCurrencyFromDeads(badGuys);
                    CleanOutDeadGuys(badGuys);
                    CleanOutDeadGuys(goodGuys);
                    if (IsDone()) return;
                }
            }
        }

        {

            ICombatant[] badGuysCopy = badGuys.ToArray();
            foreach (var combatant in badGuysCopy)
            {
                if (ItsTheirTurn(combatant) && !combatant.IsDead())
                {
                    combatant.PerformAction(goodGuys, this, evRecipient);
                    GenerateCurrencyFromDeads(badGuys);
                    CleanOutDeadGuys(badGuys);
                    CleanOutDeadGuys(goodGuys);
                    if (IsDone()) return;
                }
            }
        }
    }

    private void GenerateCurrencyFromDeads(List<ICombatant> badGuys)
    {
        badGuys.ForEach((c) =>
        {
            if (c.IsDead())
            {
                SingletonProvider.MainPlayerWallet.AddExperience(c.ExperienceWorth);
            }
        });
    }

    private bool ItsTheirTurn(ICombatant combatant)
    {
        if (combatant.TurnProgress == null) return true;
        bool itsTheirTurn = combatant.TurnProgress.IncrementTurnProgress(combatant.Attributes.Speed * SingletonProvider.MainTimeProvider.DeltaTime);
        return itsTheirTurn;
    }

    private void CleanOutDeadGuys(List<ICombatant> combatants)
    {
        List<ICombatant> deadCombatants = new List<ICombatant>();
        foreach (var item in combatants)
        {
            deadCombatants.Add(item);
        }

        combatants.RemoveAll((c) => c.IsDead());
        // When we throw event the we want the combatants list to be correct
        foreach (var item in deadCombatants)
        {
            if (item.IsDead()) evRecipient.RecieveEvent(new CombatantDied(this, item));
        }

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

    public ICombatant[] GetEnemies()
    {
        return badGuys.ToArray();
    }

    public IDisposable Subscribe(IObserver<ICombatUpdateEvent> observer)
    {
        throw new NotImplementedException();
    }
}