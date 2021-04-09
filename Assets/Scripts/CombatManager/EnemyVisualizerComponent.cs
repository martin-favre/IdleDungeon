using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualizerComponent : MonoBehaviour
{
    const string enemyGameObjectPrefab = "Prefabs/EnemyObj3D";
    SimpleObserver<ICombatUpdateEvent> observer;
    GameObject enemyObj; // will be a list

    [SerializeField]
    private Transform canvas;

    private void Start()
    {
        observer = new SimpleObserver<ICombatUpdateEvent>(CombatManager.Instance, (e) =>
        {
            if (e is EnteredCombatEvent)
            {
                Debug.Log("EnemyVisualizerComponent shows enemies");
                ShowEnemies();
            }
            else if(e is ExitedCombatEvent)
            {
                Debug.Log("EnemyVisualizerComponent hides enemies");
                HideEnemies();
            }
        });
    }

    private void ShowEnemies()
    {
        // I guess we want to read out how many enemies are here
        // ICombatReader reader = CombatManager.Instance.GetReader();
        var prefab = PrefabLoader.Instance.GetPrefab<GameObject>(enemyGameObjectPrefab);
        enemyObj = PrefabLoader.Instance.Instantiate(prefab);
        enemyObj.transform.SetParent(transform);

        var myGridPos = PlayerController.PlayerController.Instance.Position;

        var realPos = Helpers.ToVec3(myGridPos * Constants.tileSize, Constants.tileSize.y / 2);
        enemyObj.transform.position = realPos;
        enemyObj.transform.LookAt(PlayerController.PlayerController.Instance.WorldPosition);
        enemyObj.transform.Rotate(new Vector3(90, 0, 0)); // The 3d model is a unity plane which is only visible from above, need to flip it sideways.
    }

    private void HideEnemies()
    {
        if (enemyObj) GameObject.Destroy(enemyObj);
    }

    private void OnDestroy()
    {
        observer.Dispose();
    }
}