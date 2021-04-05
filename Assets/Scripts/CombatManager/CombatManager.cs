
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
    private readonly ICombatInstanceFactory combatInstanceFactory;
    private ICombatInstance combatInstance;

    List<ICombatant> playerChars;

    public CombatManager(IRandomProvider randomProvider, ICombatInstanceFactory combatInstanceFactory)
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
        this.combatInstanceFactory = combatInstanceFactory;
        playerChars = new List<ICombatant>();
        playerChars.Add(new PlayerCombatant(randomProvider));
    }

    public static void ClearInstance()
    {
        instance = null;
    }

    public IDisposable Subscribe(IObserver<CombatManagerUpdateEvent> observer)
    {
        return new SimpleUnsubscriber<CombatManagerUpdateEvent>(observers, observer);
    }

    public void PlayerEntersTile(Vector2Int tile)
    {
        if (randomProvider.ThingHappens(0.25f))
        {
            var chars = new List<ICombatant>(playerChars.ToArray());
            combatInstance = combatInstanceFactory.CreateInstance(chars);
            UpdateObservers(new CombatManagerUpdateEvent(CombatManagerUpdateEvent.UpdateType.EnteredCombat));
        }
    }

    public void Update()
    {
        if (combatInstance == null)
        {
            return;
        }

        combatInstance.Update();
        if (combatInstance.IsDone())
        {
            if (playerChars[0].IsDead())
            {
                Debug.Log("Player is dead");
                playerChars[0].Attributes.Heal(1000);
            }
            combatInstance.Dispose();
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

    public bool InCombat()
    {
        return combatInstance != null;
    }

    public ICombatReader GetReader()
    {
        return combatInstance != null ? (ICombatReader)combatInstance : null;
    }
}
