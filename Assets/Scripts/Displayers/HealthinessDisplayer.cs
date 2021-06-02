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
    }

    void UpdateValue(Upgrade healhiness)
    {
        levelHook.UpdateValue("Healhiness Level: " + Mathf.RoundToInt((float)healhiness.Level));
        costHook.UpdateValue("Healhiness Cost: " + Mathf.RoundToInt((float)healhiness.Cost));
    }
}