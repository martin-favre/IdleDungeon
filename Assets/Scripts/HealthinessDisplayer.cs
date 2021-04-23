using UnityEngine;


public class AttackinessDisplayer : MonoBehaviour
{
    SimpleValueDisplayer.ValueHook levelHook;
    SimpleValueDisplayer.ValueHook costHook;

    SimpleObserver<Upgrade> observer;

    void Awake()
    {
        levelHook = SimpleValueDisplayer.Instance.RegisterValue();
        costHook = SimpleValueDisplayer.Instance.RegisterValue();
        observer = new SimpleObserver<Upgrade>(UpgradeManager.Instance.Attackiness, UpdateValue);
        UpdateValue(UpgradeManager.Instance.Attackiness);
    }

    void UpdateValue(Upgrade attackiness)
    {
        levelHook.UpdateValue("Attackiness Level: " + attackiness.Level);
        costHook.UpdateValue("Attackiness Cost: " + attackiness.Cost);
    }
}