public class MultiplierUpgrade : Upgrade
{
    public double MultipliedValue { get => valueMultiplier * Level; }
    private readonly double valueMultiplier;

    public MultiplierUpgrade(
        double valueMultiplier,
        int initialLevel,
        float baseCost,
        float costMultiplier,
        string storageKey)
        : base(initialLevel, baseCost, costMultiplier, storageKey)
    {
        this.valueMultiplier = valueMultiplier;
    }
}