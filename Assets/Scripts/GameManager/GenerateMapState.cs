using StateMachineCollection;
using UnityEngine;

namespace GameManager
{
    public class GenerateMapState : State
    {
        private readonly IGameManager manager;

        public GenerateMapState(IGameManager manager)
        {
            
            manager.GridMap = manager.MapFactory.GenerateMap(new Vector2Int(10, 10), SingletonProvider.MainRandomProvider);
            foreach(var modifier in manager.MapModifiers) {
                modifier.ImproveMap(manager.GridMap, SingletonProvider.MainRandomProvider);
            }
            this.manager = manager;
        }
        public override State OnDuring()
        {
            return new PlayGameState(manager);
        }
    }

}