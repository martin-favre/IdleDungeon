
// Simple CombatAttributes that's just get/set
public class SimpleCombatAttributes : ICombatAttributes
{
    public int Hp { get => currentHp; }
    public int Attack { get => attack; set => attack = value; }
    public int Speed { get => speed; set => speed = value; }
    private int attack = 20;
    private int speed = 100;
    private int maxHp = 100;
    private int currentHp;

    public SimpleCombatAttributes()
    {
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
