using System;
using System.Collections.Generic;
using Logging;

public enum UpgradeType
{
    AttackinessLevel1,
    AttackinessLevel2,
    AttackinessLevel3,
    HealthinessLevel1,
    HealthinessLevel2,
    HealthinessLevel3
}

public interface IUpgradeManager : IKeyObservable<string, Upgrade>, IEventRecipient<string>
{
    void LevelUpUpgrade(string identifier);
    Upgrade GetUpgrade(string identifier);
    void SetUpgrade(string identifier, Upgrade upgrade);

    //For upgrades to publish their updates
    public void NotifyUpdate(string identifier);

}
public class UpgradeManager : IUpgradeManager
{
    static readonly UpgradeManager instance;
    Dictionary<string, Upgrade> upgrades;

    public static UpgradeManager Instance => instance;

    private static readonly LilLogger logger = new LilLogger(typeof(UpgradeManager).ToString());

    Dictionary<string, List<IObserver<Upgrade>>> observers;

    static UpgradeManager()
    {
        instance = new UpgradeManager();
    }

    public UpgradeManager()
    {
        upgrades = new Dictionary<string, Upgrade>();
        observers = new Dictionary<string, List<IObserver<Upgrade>>>();
    }
    public void LevelUpUpgrade(string identifier)
    {
        Upgrade upgrade = GetUpgrade(identifier);
        if(upgrade == null) return;
        upgrade.LevelUp();
        NotifyUpdate(identifier);
    }

    public void SetUpgrade(string identifier, Upgrade upgrade)
    {
        if (upgrades.ContainsKey(identifier))
        {
            logger.Log("Replacing upgrade " + identifier, LogLevel.Warning);
        }
        upgrades[identifier] = upgrade;
        NotifyUpdate(identifier);
    }

    public IDisposable Subscribe(string key, IObserver<Upgrade> observer)
    {
        return new KeyUnsubscriber<string, Upgrade>(observers, key, observer);
    }

    public void NotifyUpdate(string identifier)
    {
        List<IObserver<Upgrade>> obs;
        bool success = observers.TryGetValue(identifier, out obs);
        if (!success) return;

        Upgrade upgrade = GetUpgrade(identifier);
        if(upgrade == null) return;
        foreach (var observer in obs)
        {
            observer.OnNext(upgrade);
        }
    }

    public Upgrade GetUpgrade(string identifier)
    {
        Upgrade upgrade;
        bool success = upgrades.TryGetValue(identifier, out upgrade);
        if (success)
        {
            return upgrade;
        }
        else
        {
            logger.Log("Unknown Upgradetype " + identifier, LogLevel.Warning);
            return null;
        }
    }

    public void RecieveEvent(string identifier)
    {
        NotifyUpdate(identifier);
    }
}