using PlayerController;
using UnityEngine;

namespace GameManager
{

    public class GameManagerComponent : MonoBehaviour
    {
        private GameManager manager;
        private GameObject map;
        private GameObject player;

        [SerializeField]
        private FadeScreenComponent fade;

        private void Awake()
        {
            manager = new GameManager(SpawnMap, SpawnPlayer, FadeOut, FadeIn);
        }
        private void Update()
        {
            manager.Update();
        }

        void SpawnMap()
        {
            if (map != null)
            {
                GameObject.Destroy(map);
                map = null;
            }
            var mapPrefab = PrefabLoader.Instance.GetPrefab<GameObject>("Prefabs/Map");
            map = PrefabLoader.Instance.Instantiate(mapPrefab);
            map.transform.position = new Vector3(0, 0, 0);
            var comp = map.GetComponent<MapComponent>();
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
            const float initialHeight = 2f;
            player.transform.position = Helpers.ToVec3(manager.GridMap.Start, Constants.tileSize.x / 2f + initialHeight);
            var comp = player.GetComponent<PlayerControllerComponent>();
            comp.Setup(manager.GridMap, manager.OnGoalReached, manager.OnPlayerDied);
        }

        void FadeOut()
        {
            if(!fade) return;
            fade.FadeOut();
        }

        void FadeIn()
        {
            if(!fade) return;
            fade.FadeIn();
        }
    }
}