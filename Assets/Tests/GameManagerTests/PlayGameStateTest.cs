using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;
using PlayerController;
using GameManager;

namespace Tests
{
    public class PlayGameStateTest
    {

        Mock<IGridMap> mapMock;
        Vector2Int mapSize;

        Mock<IMazeGenerator> maceGeneratorMock;
        Mock<IGameManager> gameManagerMock;

        [SetUp]
        public void Setup()
        {

            mapMock = new Mock<IGridMap>();
            mapSize = new Vector2Int(20, 20);
            mapMock.Setup(foo => foo.Size).Returns(mapSize);

            maceGeneratorMock = new Mock<IMazeGenerator>();
            maceGeneratorMock.Setup(foo => foo.GenerateMap(It.IsAny<Vector2Int>(), It.IsAny<int>())).Returns(mapMock.Object);

            gameManagerMock = new Mock<IGameManager>();
            gameManagerMock.Setup(foo => foo.MapGenerator).Returns(maceGeneratorMock.Object);


        }

        [Test]
        public void ShouldSpawnObjectsOnEntry()
        {
            var state = new PlayGameState(gameManagerMock.Object);
            state.OnEntry();
            gameManagerMock.Verify (foo => foo.SpawnMaze());
            gameManagerMock.Verify (foo => foo.SpawnPlayer());
        }


        [Test]
        public void ShouldTransitionOnEvent()
        {
            var state = new PlayGameState(gameManagerMock.Object);
            state.OnEntry();
            for(int i = 0; i < 100; i++) {
                // Just run a bunch to show it's not transitioning
                Assert.IsNull(state.OnDuring()); 
            }
            state.HandleEvent(new PlayerReachedGoalEvent());
            Assert.IsTrue(state.OnDuring() is UpdatePointsState);
        }



    }
}
