using UnityEngine;

public class CombatManagerComponent : MonoBehaviour
{
    private void Awake() {
        if(CombatManager.Instance == null) {
            CombatManager.Instance = new CombatManager(SystemRandom.Instance);
        }
    }
    private void Update()
    {
        CombatManager.Instance.Update();
    }
}