using UnityEngine;


public class HealthinessDisplayer : MonoBehaviour
{
    SimpleValueDisplayer.ValueHook levelHook;
    SimpleValueDisplayer.ValueHook costHook;

    KeyObserver<string, Upgrade> observer;

    void Start()
    {
        levelHook = SimpleValueDisplayer.Instance.RegisterValue();
        costHook = SimpleValueDisplayer.Instance.RegisterValue();
        observer = new KeyObserver<string, Upgrade>(SingletonProvider.MainUpgradeManager, PlayerAttributes.GetHealthinessUpgradeKey(0, 0), UpdateValue);
    }

    void UpdateValue(Upgrade healhiness)
    {
        levelHook.UpdateValue("Healhiness Level: " + Mathf.RoundToInt((float)healhiness.Level));
        costHook.UpdateValue("Healhiness Cost: " + Mathf.RoundToInt((float)healhiness.Cost));
    }
}