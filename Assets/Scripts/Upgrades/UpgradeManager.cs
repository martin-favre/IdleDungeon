public interface IUpgradeManager
{
    Upgrade Attackiness { get; }
}
public class UpgradeManager : IUpgradeManager
{
    static readonly UpgradeManager instance;
    AttackinessUpgrade attackiness;
    HealthinessUpgrade healthiness;
    public Upgrade Attackiness => attackiness;
    public Upgrade Healthiness => healthiness;
    public static UpgradeManager Instance => instance;

    static UpgradeManager()
    {
        instance = new UpgradeManager(PlayerPrefsReader.Instance);
    }

    public UpgradeManager(IPersistentDataStorage storage)
    {
        attackiness = new AttackinessUpgrade(0, storage);
        healthiness = new HealthinessUpgrade(0, storage);
    }

}