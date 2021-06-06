
using System;
using System.Collections.Generic;

public class CombatInstance : ICombatInstance, ICombatReader
{

    List<ICharacter> goodGuys;
    List<ICharacter> badGuys;

    public ICombatReader CombatReader => this;

    public ICombatInstance.CombatResult Result
    {
        get
        {
            if (!IsDone()) return ICombatInstance.CombatResult.Unknown;
            return goodGuys.Count > 0 ? ICombatInstance.CombatResult.PlayerWon : ICombatInstance.CombatResult.PlayerLost;
        }
    }
    public CombatInstance(ICharacter[] playerChars,
    IEnemyFactory enemyFactory)
    {
        this.goodGuys = new List<ICharacter>(playerChars);
        badGuys = enemyFactory.GenerateEnemies();
        goodGuys.ForEach(e => e.TurnProgress.ResetTurnProgress());
        badGuys.ForEach(e => e.TurnProgress.ResetTurnProgress());
    }

    public void Update()
    {
        if (IsDone()) return;
        {
            ICharacter[] goodGuysCopy = goodGuys.ToArray(); // to avoid modifying the original while iterating
            foreach (var combatant in goodGuysCopy)
            {
                if (ItsTheirTurn(combatant) && !combatant.IsDead())
                {
                    combatant.PerformAction(badGuys, this);
                    GenerateCurrencyFromDeads(badGuys);
                    CleanOutDeadGuys(badGuys);
                    CleanOutDeadGuys(goodGuys);
                    if (IsDone()) return;
                }
            }
        }

        {

            ICharacter[] badGuysCopy = badGuys.ToArray();
            foreach (var combatant in badGuysCopy)
            {
                if (ItsTheirTurn(combatant) && !combatant.IsDead())
                {
                    combatant.PerformAction(goodGuys, this);
                    GenerateCurrencyFromDeads(badGuys);
                    CleanOutDeadGuys(badGuys);
                    CleanOutDeadGuys(goodGuys);
                    if (IsDone()) return;
                }
            }
        }
    }

    private void GenerateCurrencyFromDeads(List<ICharacter> badGuys)
    {
        badGuys.ForEach((c) =>
        {
            if (c.IsDead())
            {
                SingletonProvider.MainPlayerWallet.AddExperience(c.ExperienceWorth);
            }
        });
    }

    private bool ItsTheirTurn(ICharacter combatant)
    {
        if (combatant.TurnProgress == null) return true;
        bool itsTheirTurn = combatant.TurnProgress.IncrementTurnProgress(combatant.Attributes.Speed * SingletonProvider.MainTimeProvider.DeltaTime);
        return itsTheirTurn;
    }

    private void CleanOutDeadGuys(List<ICharacter> combatants)
    {
        List<ICharacter> deadCombatants = new List<ICharacter>();
        foreach (var item in combatants)
        {
            if(item.IsDead()) deadCombatants.Add(item);
        }

        combatants.RemoveAll((c) => c.IsDead());
        // When we throw event the we want the combatants list to be correct
        foreach (var item in deadCombatants)
        {
            if (item.IsDead())
            {
                MainEventHandler.Instance.Publish(EventType.CombatantDied, new CombatantDied(this, item));
            }
        }

        if(combatants == badGuys) deadCombatants.ForEach(e => e.Dispose());

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

    public ICharacter[] GetEnemies()
    {
        return badGuys.ToArray();
    }
}