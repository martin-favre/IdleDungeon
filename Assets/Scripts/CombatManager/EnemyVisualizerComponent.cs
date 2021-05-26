using System;
using System.Collections.Generic;
using Logging;
using UnityEngine;

public class EnemyVisualizerComponent : MonoBehaviour
{
    const string enemyGameObjectPrefab = "Prefabs/EnemyObj3D";
    SimpleObserver<ICombatUpdateEvent> observer;
    List<(Guid, GameObject)> enemyObjects = new List<(Guid, GameObject)>();

    [SerializeField]
    private Transform canvas;

    static readonly LilLogger logger = new LilLogger(typeof(EnemyVisualizerComponent).ToString());

    private void Start()
    {
        observer = new SimpleObserver<ICombatUpdateEvent>(CombatManager.Instance, (e) =>
        {
            if (e is EnteredCombatEvent)
            {
                logger.Log("EnemyVisualizerComponent shows enemies");
                ShowEnemies();
            }
            else if (e is ExitedCombatEvent)
            {
                logger.Log("EnemyVisualizerComponent hides enemies");
                HideEnemies();
            }
            else if (e is CombatantDied cd)
            {
                HideEnemy(cd.Victim);
            }
        });
    }

    private void HideEnemy(ICharacter victim)
    {
        foreach (var item in enemyObjects)
        {
            if (item.Item1.Equals(victim.UniqueId))
            {
                GameObject.Destroy(item.Item2);
            }
        }
    }

    private void ShowEnemies()
    {
        // I guess we want to read out how many enemies are here
        ICombatReader reader = CombatManager.Instance.GetReader();
        var attr = reader.GetEnemies();

        InstantiateEnemies(attr);
        switch (attr.Length)
        {
            case 0:
                break;
            case 1:
                PlaceOneEnemy();
                break;
            case 2:
                PlaceTwoEnemies();
                break;
            case 3:
                PlaceThreeEnemies();
                break;
            case 4:
                PlaceFourEnemies();
                break;
            default:
                logger.Log("To many enemies to show", LogLevel.Error);
                break;
        }

    }

    private void InstantiateEnemies(ICharacter[] attr)
    {
        if (enemyObjects.Count > 0) HideEnemies(); // Shouldn't happen, but just clear them out if so
        var prefab = SingletonProvider.MainGameobjectLoader.GetPrefab<GameObject>(enemyGameObjectPrefab);

        foreach (var enemy in attr)
        {
            var enemyObj = SingletonProvider.MainGameobjectLoader.Instantiate(prefab);
            enemyObj.transform.SetParent(transform);
            enemyObjects.Add((enemy.UniqueId, enemyObj));
        }
    }

    private void PlaceOneEnemy()
    {
        Debug.Assert(enemyObjects.Count >= 1);
        CentralizeEnemies();
        FinalRotate();
    }

    void FinalRotate()
    {
        // The 3d model is a unity plane which is only visible from above, need to flip it sideways.
        enemyObjects.ForEach(e => e.Item2.transform.Rotate(new Vector3(90, 0, 0)));
    }

    void CentralizeEnemies()
    {
        var myGridPos = PlayerController.Instance.Position;
        var realPos = Helpers.ToVec3(myGridPos * Constants.tileSize, Constants.tileSize.y / 2);
        enemyObjects.ForEach(e => e.Item2.transform.position = realPos);

        var playerWorldPos = PlayerController.Instance.WorldPosition;
        enemyObjects.ForEach(e => e.Item2.transform.LookAt(playerWorldPos));
    }

    private void PlaceTwoEnemies()
    {
        CentralizeEnemies();
        // Put them side by side
        {
            Debug.Assert(enemyObjects.Count >= 2);
            var leftEnemy = enemyObjects[0].Item2;
            var rightEnemy = enemyObjects[1].Item2;
            var playerWorldPos = PlayerController.Instance.WorldPosition;
            var dirToPlayer = (new Vector3(playerWorldPos.x, leftEnemy.transform.position.y, playerWorldPos.z) - leftEnemy.transform.position).normalized;
            var dirLeftOfPlayer = Quaternion.AngleAxis(90, Vector3.up) * dirToPlayer;
            leftEnemy.transform.position += dirLeftOfPlayer * (Constants.tileSize.x / 4f);
            rightEnemy.transform.position -= dirLeftOfPlayer * (Constants.tileSize.x / 4f);
        }

        FinalRotate();
    }

    private void PlaceThreeEnemies()
    {
        CentralizeEnemies();
        // Put one in front, then two side by side further back
        {
            Debug.Assert(enemyObjects.Count >= 3);
            var leftEnemy = enemyObjects[0].Item2;
            var rightEnemy = enemyObjects[1].Item2;
            var frontEnemy = enemyObjects[2].Item2;
            var playerWorldPos = PlayerController.Instance.WorldPosition;
            var dirToPlayer = (new Vector3(playerWorldPos.x, leftEnemy.transform.position.y, playerWorldPos.z) - leftEnemy.transform.position).normalized;
            var dirLeftOfPlayer = Quaternion.AngleAxis(90, Vector3.up) * dirToPlayer;
            leftEnemy.transform.position += dirLeftOfPlayer * (Constants.tileSize.x / 4f);
            leftEnemy.transform.position -= dirToPlayer * (Constants.tileSize.x / 5f);
            rightEnemy.transform.position -= dirLeftOfPlayer * (Constants.tileSize.x / 4f);
            rightEnemy.transform.position -= dirToPlayer * (Constants.tileSize.x / 5f);
        }

        FinalRotate();

    }

    private void PlaceFourEnemies()
    {
        CentralizeEnemies();
        // Put two side by side in front, then two side by side further back
        {
            Debug.Assert(enemyObjects.Count >= 4);
            var frontLeft = enemyObjects[0].Item2;
            var frontRight = enemyObjects[1].Item2;
            var backLeft = enemyObjects[2].Item2;
            var backRight = enemyObjects[3].Item2;
            var playerWorldPos = PlayerController.Instance.WorldPosition;
            var dirToPlayer = (new Vector3(playerWorldPos.x, frontLeft.transform.position.y, playerWorldPos.z) - frontLeft.transform.position).normalized;
            var dirLeftOfPlayer = Quaternion.AngleAxis(90, Vector3.up) * dirToPlayer;
            frontLeft.transform.position += dirLeftOfPlayer * (Constants.tileSize.x / 4f);
            frontRight.transform.position -= dirLeftOfPlayer * (Constants.tileSize.x / 4f);
            backLeft.transform.position += dirLeftOfPlayer * (Constants.tileSize.x / 4f);
            backLeft.transform.position -= dirToPlayer * (Constants.tileSize.x / 5f);
            backRight.transform.position -= dirLeftOfPlayer * (Constants.tileSize.x / 4f);
            backRight.transform.position -= dirToPlayer * (Constants.tileSize.x / 5f);
        }

        FinalRotate();
    }


    private void HideEnemies()
    {
        enemyObjects.ForEach(e => GameObject.Destroy(e.Item2));
        enemyObjects.Clear();
    }

    private void OnDestroy()
    {
        observer.Dispose();
    }
}