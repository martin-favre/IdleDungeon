using System;

public class HealthinessUpgrade : Upgrade
{
    public HealthinessUpgrade(int initialLevel, IPersistentDataStorage storage) : base(initialLevel, 10, 1.07f, storage) { }

    public override string StorageKey => "HealthinessLevel";
}