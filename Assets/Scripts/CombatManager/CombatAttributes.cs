
public class CombatAttributes
{
    public int Hp { get => currentHp; }
    public int Attack { get => attack; set => attack = value; }
    public int Defence { get => defence; set => defence = value; }
    public int Speed { get => speed; set => speed = value; }

    private int attack = 20;
    private int defence = 1;
    private int speed = 30;
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
