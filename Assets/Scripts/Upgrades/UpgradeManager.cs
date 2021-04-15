public interface IUpgradeManager
{
    Upgrade Attackiness { get; }
}
public class UpgradeManager : IUpgradeManager
{
    static readonly UpgradeManager instance;
    AttackinessUpgrade attackiness;
    public Upgrade Attackiness => attackiness;

    public static UpgradeManager Instance => instance;

    static UpgradeManager()
    {
        instance = new UpgradeManager(PlayerPrefsReader.Instance);
    }

    public UpgradeManager(IPersistentDataStorage storage)
    {
        attackiness = new AttackinessUpgrade(0, storage);
    }

}