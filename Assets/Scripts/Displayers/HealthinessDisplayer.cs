using UnityEngine;


public class HealthinessDisplayer : MonoBehaviour
{
    SimpleValueDisplayer.ValueHook levelHook;
    SimpleValueDisplayer.ValueHook costHook;

    SimpleObserver<Upgrade> observer;

    void Awake()
    {
        levelHook = SimpleValueDisplayer.Instance.RegisterValue();
        costHook = SimpleValueDisplayer.Instance.RegisterValue();
        observer = new SimpleObserver<Upgrade>(UpgradeManager.Instance.GetUpgrade(UpgradeType.HealthinessLevel1), UpdateValue);
        UpdateValue(UpgradeManager.Instance.GetUpgrade(UpgradeType.HealthinessLevel1));
    }

    void UpdateValue(Upgrade healhiness)
    {
        levelHook.UpdateValue("Healhiness Level: " + Mathf.RoundToInt((float)healhiness.Level));
        costHook.UpdateValue("Healhiness Cost: " + Mathf.RoundToInt((float)healhiness.Cost));
    }
}