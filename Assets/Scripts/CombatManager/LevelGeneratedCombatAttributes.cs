using UnityEngine;

public class LevelGeneratedCombatAttributes : ICombatAttributes
{
    public double MaxHp { get => maxHp; }
    public double CurrentHp { get => currentHp; }
    public double Attack { get => attack; }
    public double Speed { get => speed; }


    readonly private double attack;
    readonly private double speed;
    readonly private double maxHp;
    private double currentHp;

    public LevelGeneratedCombatAttributes(int currentLevel, float powerFactor)
    {
        // maxHp and attack should go up slightly more steeply than the player's attack and hp
        // causing so the player have to slow down and grind
        maxHp = powerFactor*Mathf.RoundToInt(20 * Mathf.Pow(1.15f, (float)currentLevel));
        attack = powerFactor*Mathf.RoundToInt(1 * Mathf.Pow(1.15f, (float)currentLevel));
        speed = 100 + currentLevel; // Idk, slowly a bit faster I guess 
        currentHp = maxHp;
    }

    public void Damage(double damage)
    {
        currentHp -= damage;
        if (currentHp <= 0) currentHp = 0;
    }

    public bool IsDead()
    {
        return currentHp <= 0;
    }

    public void Heal(double healing)
    {
        currentHp += healing;
        if (currentHp > maxHp) currentHp = maxHp;
    }
}
