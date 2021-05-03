public interface IUpgradeManager
{
    Upgrade Attackiness { get; }
    Upgrade Healthiness { get; }
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
        instance = new UpgradeManager(PlayerPrefsReader.Instance, PlayerWallet.Instance);
    }

    public UpgradeManager(IPersistentDataStorage storage, IPlayerWallet wallet)
    {
        attackiness = new AttackinessUpgrade(0, storage, wallet);
        healthiness = new HealthinessUpgrade(0, storage, wallet);
    }

}