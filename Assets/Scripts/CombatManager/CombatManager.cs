
using System;
using System.Collections.Generic;
using GameManager;
using Logging;
using UnityEngine;
public class CombatManager : ICombatManager
{

    private static CombatManager instance;
    public static CombatManager Instance { get => instance; set => instance = value; }

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

    public bool PlayerEntersTile(Vector2Int tile)
    {
        if (map.Goal == tile || (map.Start - tile).magnitude <= 1) return false;
        if (SingletonProvider.MainRandomProvider.ThingHappens(0.25f))
        {
            combatInstance = combatInstanceFactory.CreateInstance(PlayerRoster.Instance.GetAllPlayersChars());
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

    public ICombatReader GetReader()
    {
        return combatInstance != null ? combatInstance.CombatReader : null;
    }
}
