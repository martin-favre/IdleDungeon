
using System;
using System.Collections.Generic;
using Logging;
using UnityEngine;
public class CombatManager : ICombatManager
{

    private static CombatManager instance;
    public static CombatManager Instance { get => instance; set => instance = value; }

    private List<IObserver<CombatManagerUpdateEvent>> observers = new List<IObserver<CombatManagerUpdateEvent>>();
    static readonly LilLogger logger = new LilLogger("CombatManager");
    private readonly IRandomProvider randomProvider;

    private CombatInstance combatInstance;

    public CombatManager(IRandomProvider randomProvider)
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            logger.Log("Duplicate CombatManager instances", LogLevel.Error);
        }
        this.randomProvider = randomProvider;
    }

    public IDisposable Subscribe(IObserver<CombatManagerUpdateEvent> observer)
    {
        return new SimpleUnsubscriber<CombatManagerUpdateEvent>(observers, observer);
    }

    public void PlayerEntersTile(Vector2Int tile)
    {
        if (randomProvider.ThingHappens(0.25f))
        {
            combatInstance = new CombatInstance();
            UpdateObservers(new CombatManagerUpdateEvent(CombatManagerUpdateEvent.UpdateType.EnteredCombat));
        }
    }

    public void Update()
    {
        if (combatInstance == null || !combatInstance.IsDone())
        {
            return;
        }

        combatInstance.Update();
        if (combatInstance.IsDone())
        {
            combatInstance = null;
            UpdateObservers(new CombatManagerUpdateEvent(CombatManagerUpdateEvent.UpdateType.LeftCombat));
        }
    }

    private void UpdateObservers(CombatManagerUpdateEvent evt)
    {
        foreach (var observer in observers)
        {
            observer.OnNext(evt);
        }
    }


    public bool InCombat() {
        return combatInstance != null;
    }
}
