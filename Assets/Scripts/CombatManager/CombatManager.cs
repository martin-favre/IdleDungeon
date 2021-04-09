
using System;
using System.Collections.Generic;
using Logging;
using UnityEngine;
public class CombatManager : ICombatManager, IEventRecipient<ICombatUpdateEvent>
{

    private static CombatManager instance;
    public static CombatManager Instance { get => instance; set => instance = value; }

    private List<IObserver<ICombatUpdateEvent>> observers = new List<IObserver<ICombatUpdateEvent>>();
    static readonly LilLogger logger = new LilLogger("CombatManager");
    private readonly IRandomProvider randomProvider;
    private readonly ICombatInstanceFactory combatInstanceFactory;
    private readonly IMap map;
    private ICombatInstance combatInstance;

    public CombatManager(IRandomProvider randomProvider, ICombatInstanceFactory combatInstanceFactory, IMap map)
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
        this.map = map;
    }

    public static void ClearInstance()
    {
        instance = null;
    }

    public IDisposable Subscribe(IObserver<ICombatUpdateEvent> observer)
    {
        return new SimpleUnsubscriber<ICombatUpdateEvent>(observers, observer);
    }

    public bool PlayerEntersTile(Vector2Int tile)
    {
        if(map.Goal == tile || (map.Start - tile).magnitude <= 1) return false;
        if (randomProvider.ThingHappens(0.25f))
        {
            combatInstance = combatInstanceFactory.CreateInstance(PlayerCharacters.Instance.GetAllPlayersChars(), this);
            UpdateObservers(new EnteredCombatEvent((ICombatReader)combatInstance));
            return true;
        }
        return false;
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
            combatInstance.Dispose();
            combatInstance = null;
            UpdateObservers(new ExitedCombatEvent(null));
        }
    }

    private void UpdateObservers(ICombatUpdateEvent evt)
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

    public void RecieveEvent(ICombatUpdateEvent ev)
    {
        UpdateObservers(ev);
    }
}
