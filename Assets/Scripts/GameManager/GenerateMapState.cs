using StateMachineCollection;
using UnityEngine;

namespace GameManager
{
    public class GenerateMapState : State
    {
        private readonly IMaceGenerator maceGenerator;
        private readonly IGameManager manager;

        public GenerateMapState(IGameManager manager)
        {
            manager.GridMap = manager.MapGenerator.GenerateMap(new Vector2Int(20, 20), Random.Range(0, int.MaxValue));
            this.manager = manager;
        }
        public override State OnDuring()
        {
            TerminateMachine();
            return new PlayGameState(manager);
        }
    }

}