using UnityEngine;

public class EnemyVisualizerComponent : MonoBehaviour
{
    SimpleObserver<CombatManagerUpdateEvent> observer;
    private void Start()
    {
        observer = new SimpleObserver<CombatManagerUpdateEvent>(CombatManager.Instance, (e) =>
        {
            if (e.Type == CombatManagerUpdateEvent.UpdateType.EnteredCombat)
            {
                Debug.Log("EnemyVisualizerComponent shows enemies");
            } else {
                Debug.Log("EnemyVisualizerComponent hides enemies");
            }
        });
    }

    private void OnDestroy() {
        observer.Dispose();
    }
}