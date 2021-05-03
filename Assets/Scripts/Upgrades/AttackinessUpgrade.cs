using System;

public class AttackinessUpgrade : Upgrade
{
    public AttackinessUpgrade(int initialLevel, IPersistentDataStorage storage, IPlayerWallet wallet) : base(initialLevel, 10, 1.10f, storage, wallet) { }

    public override string StorageKey => "AttackinessLevel";
}