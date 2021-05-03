using System;

public class HealthinessUpgrade : Upgrade
{
    public HealthinessUpgrade(int initialLevel, IPersistentDataStorage storage, IPlayerWallet wallet) : base(initialLevel, 10, 1.07f, storage, wallet) { }

    public override string StorageKey => "HealthinessLevel";
}