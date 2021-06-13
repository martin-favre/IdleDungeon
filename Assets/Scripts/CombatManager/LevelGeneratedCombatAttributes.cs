using System;
using UnityEngine;

public class LevelGeneratedCombatAttributes : ICombatAttributes
{
    public double Attack { get => attack; }
    public double Speed { get => speed; }

    readonly private double attack;
    readonly private double speed;

    public LevelGeneratedCombatAttributes(int currentLevel, float powerFactor)
    {
        // maxHp and attack should go up slightly more steeply than the player's attack and hp
        // causing so the player have to slow down and grind
        attack = powerFactor * Mathf.RoundToInt(1 * Mathf.Pow(1.15f, (float)currentLevel));
        speed = 100 + currentLevel; // Idk, slowly a bit faster I guess 
    }
}
