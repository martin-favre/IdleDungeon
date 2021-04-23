using UnityEngine;
public class IncreaseUpgradeButton : MonoBehaviour
{
    public void IncreaseAttackiness()
    {
        UpgradeManager.Instance.Attackiness.LevelUp();
    }
    public void IncreaseHealthiness()
    {
        UpgradeManager.Instance.Healthiness.LevelUp();
    }

}