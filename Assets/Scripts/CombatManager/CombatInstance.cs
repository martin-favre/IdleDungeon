
using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatInstance : ICombatInstance, ICombatReader
{

    private readonly List<ICharacter> goodGuys;
    private readonly List<ICharacter> badGuys;
    double gainedGold;
    public ICombatReader CombatReader => this;

    public CombatInstance(ICharacter[] playerChars,
    IEnemyFactory enemyFactory)
    {
        this.goodGuys = new List<ICharacter>(playerChars);
        badGuys = enemyFactory.GenerateEnemies();
    }

    private CombatResult GenerateResult()
    {
        return new CombatResult(Mathf.RoundToInt((float)gainedGold), goodGuys.Count > 0);
    }

    public CombatResult Update()
    {
        if (IsDone()) return GenerateResult();
        {
            ICharacter[] goodGuysCopy = goodGuys.ToArray(); // to avoid modifying the original while iterating
            foreach (var combatant in goodGuysCopy)
            {
                if (!combatant.IsDead())
                {
                    combatant.PerformAction(badGuys, this);
                    GenerateCurrencyFromDeads(badGuys);
                    CleanOutDeadGuys(badGuys);
                    CleanOutDeadGuys(goodGuys);
                    if (IsDone()) return GenerateResult();
                }
            }
        }

        {

            ICharacter[] badGuysCopy = badGuys.ToArray();
            foreach (var combatant in badGuysCopy)
            {
                if (!combatant.IsDead())
                {
                    combatant.PerformAction(goodGuys, this);
                    GenerateCurrencyFromDeads(badGuys);
                    CleanOutDeadGuys(badGuys);
                    CleanOutDeadGuys(goodGuys);
                    if (IsDone()) return GenerateResult();
                }
            }
        }
        return null;
    }
    private void GenerateCurrencyFromDeads(List<ICharacter> badGuys)
    {
        badGuys.ForEach((c) =>
        {
            if (c.IsDead())
            {
                gainedGold += c.GoldWorth;
            }
        });
    }

    private void CleanOutDeadGuys(List<ICharacter> combatants)
    {
        List<ICharacter> deadCombatants = new List<ICharacter>();
        foreach (var item in combatants)
        {
            if (item.IsDead()) deadCombatants.Add(item);
        }

        combatants.RemoveAll((c) => c.IsDead());
        // When we throw event the we want the combatants list to be correct
        foreach (var item in deadCombatants)
        {
            if (item.IsDead())
            {
                CentralEventPublisher.Instance.Publish(EventType.CombatantDied, new CombatantDied(this, item));
            }
        }
    }

    private bool IsDone()
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

    public ICharacter[] GetPlayers()
    {
        return goodGuys.ToArray();
    }
}