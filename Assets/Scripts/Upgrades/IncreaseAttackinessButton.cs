using UnityEngine;
public class IncreaseAttackinessButton : MonoBehaviour
{
    public void IncreaseAttackiness()
    {
        UpgradeManager.Instance.Attackiness.LevelUp();
    }
}