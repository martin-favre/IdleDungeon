using UnityEngine;

public class LevelGeneratedCombatAttributes : ICombatAttributes
{
    public int MaxHp { get => maxHp; }
    public int CurrentHp { get => currentHp; }
    public int Attack { get => attack; }
    public int Speed { get => speed; }


    readonly private int attack;
    readonly private int speed;
    readonly private int maxHp;
    private int currentHp;

    public LevelGeneratedCombatAttributes(int currentLevel)
    {
        // maxHp and attack should go up slightly more steeply than the player's attack and hp
        // causing so the player have to slow down and grind
        maxHp = Mathf.RoundToInt(10 * Mathf.Pow(1.15f, (float)currentLevel));
        attack = Mathf.RoundToInt(1 * Mathf.Pow(1.15f, (float)currentLevel));
        speed = 100 + currentLevel; // Idk, slowly a bit faster I guess 
        currentHp = maxHp;
    }

    public void Damage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0) currentHp = 0;
    }

    public bool IsDead()
    {
        return currentHp <= 0;
    }

}
