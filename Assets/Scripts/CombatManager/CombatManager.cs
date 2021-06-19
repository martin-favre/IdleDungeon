
using System;
using System.Collections.Generic;
using GameManager;
using Logging;
using UnityEngine;
public class CombatManager : ICombatManager
{

    private static CombatManager instance;
    public static CombatManager Instance { get => instance; set => instance = value; }

    public ICombatReader CombatReader { get => combatInstance != null ? combatInstance.CombatReader : null; }

    static readonly LilLogger logger = new LilLogger("CombatManager");
    private readonly ICombatInstanceFactory combatInstanceFactory;
    private ICombatInstance combatInstance;

    static CombatManager()
    {
        instance = new CombatManager(new CombatInstanceFactory());
    }

    public CombatManager(ICombatInstanceFactory combatInstanceFactory)
    {
        this.combatInstanceFactory = combatInstanceFactory;
    }

    public static void ClearInstance()
    {
        instance = null;
    }

    public bool PlayerEntersTile(Vector2Int tile)
    {
        var map = SingletonProvider.MainGameManager.GridMap;
        if (map.Goal == tile || (map.Start - tile).magnitude <= 1) return false;
        if (SingletonProvider.MainRandomProvider.ThingHappens(0.25f))
        {
            combatInstance = combatInstanceFactory.CreateInstance(SingletonProvider.MainPlayerRoster.GetAllPlayersChars());
            CentralEventHandler.Instance.Publish(EventType.CombatStarted, new CombatStartedEvent(combatInstance.CombatReader));
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
            var result = combatInstance.Result == ICombatInstance.CombatResult.PlayerLost ? CombatEndedEvent.CombatResult.PlayerLost : CombatEndedEvent.CombatResult.PlayerWon;
            combatInstance.Dispose();
            combatInstance = null;
            CentralEventHandler.Instance.Publish(EventType.CombatEnded, new CombatEndedEvent(null, result));
        }
    }

    public bool InCombat()
    {
        return combatInstance != null;
    }
}
