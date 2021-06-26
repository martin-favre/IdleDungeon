public class DamageConfig{
    private readonly int minimumDamage;
    private readonly int maximumDamage;

    public DamageConfig(int minimumDamage, int maximumDamage)
    {
        this.minimumDamage = minimumDamage;
        this.maximumDamage = maximumDamage;
    }

    public int MinimumDamage => minimumDamage;

    public int MaximumDamage => maximumDamage;
}