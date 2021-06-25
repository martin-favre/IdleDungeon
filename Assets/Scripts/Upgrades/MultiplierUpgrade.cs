public class MultiplierUpgrade : Upgrade
{
    public float MultipliedValue { get => valueMultiplier * Level; }

    private readonly float valueMultiplier;

    public MultiplierUpgrade(
        float valueMultiplier,
        int initialLevel,
        float baseCost,
        float costMultiplier,
        string storageKey)
        : base(initialLevel, baseCost, costMultiplier, storageKey)
    {
        this.valueMultiplier = valueMultiplier;
    }

    public double GetMultipliedValueAtLevel(int level)
    {
        return level * valueMultiplier;
    }
}