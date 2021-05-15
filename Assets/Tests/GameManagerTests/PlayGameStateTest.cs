using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;

using GameManager;

namespace Tests
{
    public class PlayGameStateTest
    {

        Mock<IMap> mapMock;
        Vector2Int mapSize;
 
        Mock<IMapFactory> mapGeneratorMock;
        Mock<IGameManager> gameManagerMock;

        Mock<ITimeProvider> timeProviderMock;

        [SetUp]
        public void Setup()
        {

            mapMock = new Mock<IMap>();
            mapSize = new Vector2Int(20, 20);
            mapMock.Setup(foo => foo.Size).Returns(mapSize);

            mapGeneratorMock = new Mock<IMapFactory>();
            mapGeneratorMock.Setup(foo => foo.GenerateMap(It.IsAny<Vector2Int>(), It.IsAny<IRandomProvider>())).Returns(mapMock.Object);

            timeProviderMock = new Mock<ITimeProvider>();

            gameManagerMock = new Mock<IGameManager>();
            gameManagerMock.Setup(foo => foo.MapFactory).Returns(mapGeneratorMock.Object);
        }

        [Test]
        public void ShouldSpawnObjectsOnEntry()
        {
            var state = new PlayGameState(gameManagerMock.Object);
            state.OnEntry();
            gameManagerMock.Verify (foo => foo.SpawnMap());
            gameManagerMock.Verify (foo => foo.SpawnPlayer());
            gameManagerMock.Verify (foo => foo.FadeIn());
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
            Assert.IsTrue(state.OnDuring() is FadeOutState);
        }



    }
}
