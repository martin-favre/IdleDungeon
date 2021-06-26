public class AttackConfig
{
    private readonly bool isReflected;
    private readonly float damage;

    public AttackConfig(float damage = 0, bool isReflected = false)
    {
        this.isReflected = isReflected;
        this.damage = damage;
    }

    public bool IsReflected => isReflected;

    public float Damage => damage;
}