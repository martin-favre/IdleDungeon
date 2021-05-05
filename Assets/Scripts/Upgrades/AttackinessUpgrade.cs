using System;

public class AttackinessUpgrade : Upgrade
{
    public AttackinessUpgrade(int initialLevel, IPersistentDataStorage storage, IPlayerWallet wallet) : base(initialLevel, 15, 1.15f, storage, wallet) { }

    public override string StorageKey => "AttackinessLevel";
}