public class EnemyAttributes : ICombatAttributes
{
    private readonly float attack;
    private readonly float speed;

    public EnemyAttributes(float attack, float speed)
    {
        this.attack = attack;
        this.speed = speed;
    }
    public float Attack => attack;

    public float Speed => speed;
}