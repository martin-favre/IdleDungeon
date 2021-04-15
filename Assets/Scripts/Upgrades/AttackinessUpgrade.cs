using System;

public class AttackinessUpgrade : Upgrade
{
    public AttackinessUpgrade(int initialLevel, IPersistentDataStorage storage) : base(initialLevel, 10, 1.07f, storage) { }

    public override string StorageKey => "AttackinessLevel";
}