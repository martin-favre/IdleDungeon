public class MultiplierUpgrade : Upgrade
{
    public double MultipliedValue { get => valueMultiplier * Level; }
    private readonly double valueMultiplier;

    public MultiplierUpgrade(
        double valueMultiplier,
        int initialLevel,
        float baseCost,
        float costMultiplier,
        string storageKey,
        IPersistentDataStorage storage,
        IPlayerWallet wallet,
        IUpgradeManager upgradeManager)
        : base(initialLevel, baseCost, costMultiplier, storageKey, storage, wallet, upgradeManager)
    {
        this.valueMultiplier = valueMultiplier;
    }
}