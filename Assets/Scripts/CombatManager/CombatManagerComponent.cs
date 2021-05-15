using UnityEngine;

public class CombatManagerComponent : MonoBehaviour
{
    private void Awake()
    {
        if (CombatManager.Instance == null)
        {
            // Will be executed after GameManagerComponent thanks to Unity Settings
            CombatManager.Instance = new CombatManager(new CombatInstanceFactory(), GameManager.GameManager.Instance.GridMap);
        }
    }

    private void Setup()
    {

    }
    private void Update()
    {
        CombatManager.Instance.Update();
    }
}