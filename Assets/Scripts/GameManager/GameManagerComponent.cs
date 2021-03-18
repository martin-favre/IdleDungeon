using PlayerController;
using UnityEngine;

namespace GameManager
{

    public class GameManagerComponent : MonoBehaviour
    {
        private GameManager manager;
        private GameObject maze;
        private GameObject player;

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
            if (maze != null)
            {
                GameObject.Destroy(maze);
                maze = null;
            }
            var mazePrefab = PrefabLoader.Instance.GetPrefab<GameObject>("Prefabs/Maze");
            maze = PrefabLoader.Instance.Instantiate(mazePrefab);
            maze.transform.position = new Vector3(0, 0, 0);
            var comp = maze.GetComponent<MazeComponent>();
            comp.Setup(manager.GridMap);
        }

        void SpawnPlayer()
        {
            if (player != null)
            {
                GameObject.Destroy(player);
                player = null;
            }

            var playerPrefab = PrefabLoader.Instance.GetPrefab<GameObject>("Prefabs/Player");
            player = PrefabLoader.Instance.Instantiate(playerPrefab);
            player.transform.position = new Vector3(1, Constants.tileSize.x / 2f, 1);
            var comp = player.GetComponent<PlayerControllerComponent>();
            comp.Setup(manager.GridMap, manager.OnGoalReached);
        }
    }
}