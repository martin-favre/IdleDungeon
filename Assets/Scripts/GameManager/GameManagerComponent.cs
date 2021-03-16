using PlayerController;
using UnityEngine;

namespace GameManager
{

    public class GameManagerComponent : MonoBehaviour
    {
        GameManager manager;

        private void Awake()
        {
            manager = new GameManager(SpawnMaze, SpawnPlayer);
        }
        private void Update()
        {
            manager.Update();
        }

        void SpawnMaze()
        {
            var mazePrefab = PrefabLoader.Instance.GetPrefab<GameObject>("Prefabs/Maze");
            var player = PrefabLoader.Instance.Instantiate(mazePrefab);
            player.transform.position = new Vector3(0, 0, 0);
        }

        void SpawnPlayer()
        {
            var playerPrefab = PrefabLoader.Instance.GetPrefab<GameObject>("Prefabs/Player");
            var player = PrefabLoader.Instance.Instantiate(playerPrefab);
            player.transform.position = new Vector3(0, Constants.tileSize.x/2f, 0);
            var comp = player.GetComponent<PlayerControllerComponent>();
            comp.Setup(manager.GridMap, manager.OnGoalReached);
        }
    }
}