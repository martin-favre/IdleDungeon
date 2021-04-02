using System;
using UnityEngine;

public class CombatManagerUpdateEvent
{
    public enum UpdateType
    {
        EnteredCombat,
        LeftCombat
    }

    private readonly UpdateType updateType;
    public UpdateType Type => updateType;

    public CombatManagerUpdateEvent(UpdateType updateType)
    {
        this.updateType = updateType;
    }

}

public interface ICombatManager : IObservable<CombatManagerUpdateEvent>
{
    void PlayerEntersTile(Vector2Int tile);
    bool InCombat();
}
