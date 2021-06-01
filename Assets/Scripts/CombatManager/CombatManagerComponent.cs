using UnityEngine;

public class CombatManagerComponent : MonoBehaviour
{

    private void Update()
    {
        CombatManager.Instance.Update();
    }
}