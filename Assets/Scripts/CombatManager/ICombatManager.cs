using System;
using UnityEngine;

public interface ICombatManager : IObservable<ICombatUpdateEvent>
{
    bool PlayerEntersTile(Vector2Int tile);
    bool InCombat();
}
