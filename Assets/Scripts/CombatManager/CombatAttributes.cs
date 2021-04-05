
public class CombatAttributes
{
    public int Attack { get => 2; }
    public int Defence { get => 1; }
    public int Speed { get => 1; }

    public int Hp { get => currentHp; }

    private int maxHp = 100;
    private int currentHp;

    public CombatAttributes()
    {
        currentHp = maxHp;
    }

    public int CalculateDamage(int attack)
    {
        return attack - Defence;
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

    public void Heal(int damage)
    {
        currentHp += damage;
        if (currentHp > maxHp) currentHp = maxHp;
    }
}
