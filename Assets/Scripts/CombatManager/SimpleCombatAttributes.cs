
// Simple CombatAttributes that's just get/set
public class SimpleCombatAttributes : ICombatAttributes
{
    public double MaxHp { get => maxHp; }
    public double CurrentHp { get => currentHp; }
    public double Attack { get => attack; set => attack = value; }
    public double Speed { get => speed; set => speed = value; }
    private double attack = 20;
    private double speed = 100;
    private double maxHp = 100;
    private double currentHp;

    public SimpleCombatAttributes()
    {
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

}
