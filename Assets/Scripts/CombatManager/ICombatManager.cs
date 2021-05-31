using System;
using UnityEngine;

public interface ICombatManager
{
    bool PlayerEntersTile(Vector2Int tile);
    bool InCombat();

    ICombatReader CombatReader { get; }
}
