public class EnemyAttributes : ICombatAttributes
{
    private readonly double attack;
    private readonly double speed;

    public EnemyAttributes(double attack, double speed)
    {
        this.attack = attack;
        this.speed = speed;
    }
    public double Attack => attack;

    public double Speed => speed;
}