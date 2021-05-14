using UnityEngine;


public class AttackinessDisplayer : MonoBehaviour
{
    SimpleValueDisplayer.ValueHook levelHook;
    SimpleValueDisplayer.ValueHook costHook;

    KeyObserver<string, Upgrade> observer;

    void Start()
    {
        levelHook = SimpleValueDisplayer.Instance.RegisterValue();
        costHook = SimpleValueDisplayer.Instance.RegisterValue();
        observer = new KeyObserver<string, Upgrade>(UpgradeManager.Instance, PlayerAttributes.GetAttackinessUpgradeKey(0, 0), UpdateValue);
    }

    void UpdateValue(Upgrade attackiness)
    {
        levelHook.UpdateValue("Attackiness Level: " + Mathf.RoundToInt((float)attackiness.Level));
        costHook.UpdateValue("Attackiness Cost: " + Mathf.RoundToInt((float)attackiness.Cost));
    }
}