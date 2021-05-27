
using System;
using System.Collections.Generic;
using GameManager;
using Logging;
using UnityEngine;
public class CombatManager : ICombatManager, IEventRecipient<ICombatUpdateEvent>
{

    private static CombatManager instance;
    public static CombatManager Instance { get => instance; set => instance = value; }

    private List<IObserver<ICombatUpdateEvent>> observers = new List<IObserver<ICombatUpdateEvent>>();
    static readonly LilLogger logger = new LilLogger("CombatManager");
    private readonly ICombatInstanceFactory combatInstanceFactory;
    private readonly IMap map;
    private ICombatInstance combatInstance;

    public CombatManager(ICombatInstanceFactory combatInstanceFactory, IMap map)
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            logger.Log("Duplicate CombatManager instances", LogLevel.Error);
        }
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
        if (map.Goal == tile || (map.Start - tile).magnitude <= 1) return false;
        if (SingletonProvider.MainRandomProvider.ThingHappens(0.25f))
        {
            combatInstance = combatInstanceFactory.CreateInstance(PlayerRoster.Instance.GetAllPlayersChars(), this);
            UpdateObservers(new EnteredCombatEvent(combatInstance.CombatReader));
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
            var result = combatInstance.Result == ICombatInstance.CombatResult.PlayerLost ? ExitedCombatEvent.CombatResult.PlayerLost : ExitedCombatEvent.CombatResult.PlayerWon;
            if (result == ExitedCombatEvent.CombatResult.PlayerWon)
            {
                foreach (var character in SingletonProvider.MainPlayerRoster.GetAllPlayersChars()) {
                    character.Attributes.Heal(character.Attributes.MaxHp*0.01);
                }
            }
            combatInstance.Dispose();
            combatInstance = null;
            UpdateObservers(new ExitedCombatEvent(null, result));
        }
    }

    private void UpdateObservers(ICombatUpdateEvent evt)
    {
        var oldObservers = observers.ToArray();
        foreach (var observer in oldObservers)
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
        return combatInstance != null ? combatInstance.CombatReader : null;
    }

    public void RecieveEvent(ICombatUpdateEvent ev)
    {
        UpdateObservers(ev);
    }
}
