
using System;
using System.Threading.Tasks;
using StateMachineCollection;
using UnityEngine;

namespace GameManager
{

    public class GameManager : IGameManager
    {
        private readonly IMapFactory mapGenerator = new BlockyRecursiveBacktracker();
        private readonly IMapModifier[] mapModifiers = {
            new GoalAndStartAdder()
        };
        private IMap map;
        private readonly Action spawnMap;
        private readonly Action spawnPlayer;

        private StateMachine machine;
 
        public IMap GridMap { get => map; set => map = value; }
        public IMapFactory MapFactory { get => mapGenerator; }
        public IMapModifier[] MapModifiers { get => mapModifiers; }

        public GameManager(Action spawnMap, Action spawnPlayer)
        {
            this.spawnMap = spawnMap;
            this.spawnPlayer = spawnPlayer;
            machine = new StateMachine(new GenerateMapState(this));
        }

        public void OnGoalReached()
        {
            machine.RaiseEvent(new PlayerReachedGoalEvent());
        }

        public void Update()
        {
            if (!machine.IsTerminated())
            {
                machine.Update();
            }
        }

        public void SpawnMap()
        {
            this.spawnMap();
        }

        public void SpawnPlayer()
        {
            this.spawnPlayer();
        }
    }
}