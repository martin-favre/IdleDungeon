
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
    private IMap map;
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
        if (map == null)
        {
            map = SingletonProvider.MainGameManager.GridMap;
        }
        if (map.Goal == tile || (map.Start - tile).magnitude <= 1) return false;
        if (SingletonProvider.MainRandomProvider.ThingHappens(0.25f))
        {
            combatInstance = combatInstanceFactory.CreateInstance(SingletonProvider.MainPlayerRoster.GetAllPlayersChars());
            CombatEventPublisher.Instance.Publish(new EnteredCombatEvent(combatInstance.CombatReader));
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
                foreach (var character in SingletonProvider.MainPlayerRoster.GetAllPlayersChars())
                {
                    character.Attributes.Heal(character.Attributes.MaxHp * 0.01);
                }
            }
            combatInstance.Dispose();
            combatInstance = null;
            CombatEventPublisher.Instance.Publish(new ExitedCombatEvent(null, result));
        }
    }

    public bool InCombat()
    {
        return combatInstance != null;
    }
}
