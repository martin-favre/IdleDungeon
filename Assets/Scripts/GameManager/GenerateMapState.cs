using StateMachineCollection;
using UnityEngine;

namespace GameManager
{
    public class GenerateMapState : State
    {
        private readonly IGameManager manager;

        public GenerateMapState(IGameManager manager)
        {
            int seed = Random.Range(0, int.MaxValue);
            manager.GridMap = manager.MapFactory.GenerateMap(new Vector2Int(20, 20), Random.Range(0, int.MaxValue));
            foreach(var modifier in manager.MapModifiers) {
                modifier.ImproveMap(manager.GridMap, seed);
            }
            this.manager = manager;
        }
        public override State OnDuring()
        {
            return new PlayGameState(manager);
        }
    }

}