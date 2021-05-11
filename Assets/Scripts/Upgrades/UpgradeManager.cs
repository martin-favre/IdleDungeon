using System.Collections.Generic;
using Logging;

public enum UpgradeType
{
    AttackinessLevel1,
    AttackinessLevel2,
    AttackinessLevel3,
    HealthinessLevel1
}

public interface IUpgradeManager
{

    Upgrade GetUpgrade(UpgradeType type);
}
public class UpgradeManager : IUpgradeManager
{
    static readonly UpgradeManager instance;
    Dictionary<UpgradeType, Upgrade> upgrades;

    public static UpgradeManager Instance => instance;

    private static readonly LilLogger logger = new LilLogger(typeof(UpgradeManager).ToString());

    static UpgradeManager()
    {
        instance = new UpgradeManager(PlayerPrefsReader.Instance, PlayerWallet.Instance);
    }

    public UpgradeManager(IPersistentDataStorage storage, IPlayerWallet wallet)
    {
        upgrades = new Dictionary<UpgradeType, Upgrade>
        {
            {UpgradeType.AttackinessLevel1, new Upgrade(0, 50, 1.07f, "attackinessLevel1", storage, wallet)},
            {UpgradeType.AttackinessLevel2, new Upgrade(0, 1000, 1.09f, "attackinessLevel2", storage, wallet)},
            {UpgradeType.AttackinessLevel3, new Upgrade(0, 10000, 1.11f, "attackinessLevel3", storage, wallet)},
            {UpgradeType.HealthinessLevel1, new Upgrade(0, 50, 1.07f, "healthinessLevel1", storage, wallet)}
        };
    }
    public Upgrade GetUpgrade(UpgradeType type)
    {
        Upgrade upgrade;
        bool success = upgrades.TryGetValue(type, out upgrade);
        if (!success)
        {
            logger.Log("Unknown Upgradetype " + type.ToString(), LogLevel.Warning);
            return null;
        }
        return upgrade;

    }
}