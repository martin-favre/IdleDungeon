
using System;
using System.Threading.Tasks;
using StateMachineCollection;
using UnityEngine;

namespace GameManager
{

    public class GameManager : IGameManager
    {
        private IMaceGenerator maceGenerator = new RecursiveBacktracker();
        private IGridMap map;
        private readonly Action spawnMaze;
        private readonly Action spawnPlayer;

        StateMachine machine;

        public IGridMap GridMap { get => map; set => map = value; }
        public IMaceGenerator MapGenerator { get => maceGenerator; set => maceGenerator = value; }
        public GameManager(Action spawnMaze, Action spawnPlayer)
        {
            this.spawnMaze = spawnMaze;
            this.spawnPlayer = spawnPlayer;
            machine = new StateMachine(new GenerateMapState(this));
        }

        public void OnGoalReached() {
            machine.RaiseEvent(new PlayerReachedGoalEvent());
        }

        public void Update()
        {
            if (!machine.IsTerminated())
            {
                machine.Update();
            }
        }

        public void SpawnMaze()
        {
            this.spawnMaze();
        }

        public void SpawnPlayer()
        {
            this.spawnPlayer();
        }
    }
}