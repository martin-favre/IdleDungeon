
using UnityEngine;

namespace GameManager
{
    public interface IGameManagerComponent
    {
        void SpawnMap();
        void SpawnPlayer();
        void FadeIn();
        void FadeOut();
    }

    public class GameManagerComponent : MonoBehaviour, IGameManagerComponent
    {
        private GameManager manager;
        private GameObject map;
        private GameObject player;

        [SerializeField]
        private FadeScreenComponent fade;

        private void Awake()
        {
            manager = new GameManager(this);
        }
        private void Update()
        {
            manager.Update();
        }

        public void SpawnMap()
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

        public void SpawnPlayer()
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
            comp.Setup(manager.GridMap, manager);
        }

        public void FadeOut()
        {
            if (!fade) return;
            fade.FadeOut();
        }

        public void FadeIn()
        {
            if (!fade) return;
            fade.FadeIn();
        }
    }
}