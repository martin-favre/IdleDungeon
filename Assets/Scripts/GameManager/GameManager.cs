
using System;
using System.Threading.Tasks;
using StateMachineCollection;
using UnityEngine;

namespace GameManager
{

    public interface PlayerCallbacks
    {
        void OnPlayerDied();
        void OnPlayerReachedGoal();
    }
    public class GameManager : IGameManager, PlayerCallbacks
    {
        private readonly IMapFactory mapGenerator = new BlockyRecursiveBacktracker();
        private readonly IMapModifier[] mapModifiers = {
            new GoalAndStartAdder()
        };
        private IMap map;
        private readonly Action spawnMap;
        private readonly Action spawnPlayer;
        private readonly Action fadeOut;
        private readonly Action fadeIn;
        private readonly IGameManagerComponent component;
        private StateMachine machine;
        public IMap GridMap { get => map; set => map = value; }
        public IMapFactory MapFactory { get => mapGenerator; }
        public IMapModifier[] MapModifiers { get => mapModifiers; }
        public ITimeProvider TimeProvider { get => UnityTime.Instance; }
        public IRandomProvider RandomProvider { get => SystemRandom.Instance; }
        public IPersistentDataStorage DataStorage { get => PlayerPrefsReader.Instance; }
        static IGameManager instance;
        public static IGameManager Instance { get => instance; }
        public IPlayerCharacters PlayerChars { get => PlayerCharacters.Instance; }

        public GameManager(IGameManagerComponent component)
        {
            machine = new StateMachine(new GenerateMapState(this));
            instance = this;
            this.component = component;
        }

        public void OnPlayerReachedGoal()
        {
            machine.RaiseEvent(new PlayerReachedGoalEvent());
        }
        public void OnPlayerDied()
        {
            machine.RaiseEvent(new PlayerDiedEvent());
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
            component.SpawnMap();
        }

        public void SpawnPlayer()
        {
            component.SpawnPlayer();
        }

        public void FadeOut()
        {
            component.FadeOut();
        }

        public void FadeIn()
        {
            component.FadeIn();
        }
    }
}