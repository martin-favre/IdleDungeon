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
        observer = new SimpleObserver<Upgrade>(UpgradeManager.Instance.Healthiness, UpdateValue);
        UpdateValue(UpgradeManager.Instance.Healthiness);
    }

    void UpdateValue(Upgrade healhiness)
    {
        levelHook.UpdateValue("Healhiness Level: " + healhiness.Level);
        costHook.UpdateValue("Healhiness Cost: " + healhiness.Cost);
    }
}