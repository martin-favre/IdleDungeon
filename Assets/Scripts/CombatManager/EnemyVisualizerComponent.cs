using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualizerComponent : MonoBehaviour
{
    const string enemyGameObjectPrefab = "Prefabs/EnemyObj3D";
    SimpleObserver<CombatManagerUpdateEvent> observer;
    GameObject enemyObj; // will be a list

    [SerializeField]
    private Transform canvas;

    private void Start()
    {
        observer = new SimpleObserver<CombatManagerUpdateEvent>(CombatManager.Instance, (e) =>
        {
            if (e.Type == CombatManagerUpdateEvent.UpdateType.EnteredCombat)
            {
                Debug.Log("EnemyVisualizerComponent shows enemies");
                ShowEnemies();
            }
            else
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

        var playerGridPos = PlayerController.PlayerController.Instance.Position;
        var movementDir = PlayerController.PlayerController.Instance.MovementDir;
        var myGridPos = playerGridPos + movementDir;

        var realPos = Helpers.ToVec3(myGridPos * Constants.tileSize, Constants.tileSize.y / 2);
        var playerRealPos = Helpers.ToVec3(playerGridPos * Constants.tileSize, Constants.tileSize.y / 2);
        enemyObj.transform.position = realPos;
        enemyObj.transform.LookAt(playerRealPos);
        enemyObj.transform.Rotate(new Vector3(90, 0, 0));
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