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
    public class FadeOutStateTest
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
            mapGeneratorMock.Setup(foo => foo.GenerateMap(It.IsAny<Vector2Int>(), It.IsAny<int>())).Returns(mapMock.Object);
            
            timeProviderMock = new Mock<ITimeProvider>();
            timeProviderMock.Setup(foo => foo.Time).Returns(0);

            gameManagerMock = new Mock<IGameManager>();
            gameManagerMock.Setup(foo => foo.MapFactory).Returns(mapGeneratorMock.Object);
            gameManagerMock.Setup(foo => foo.TimeProvider).Returns(timeProviderMock.Object);

        }

        [Test]
        public void ShouldSpawnObjectsOnEntry()
        {
            var state = new FadeOutState(gameManagerMock.Object);
            state.OnEntry();
            gameManagerMock.Verify (foo => foo.FadeOut());
        }


        [Test]
        public void ShouldTransitionOnAfterAWhile()
        {
            var state = new FadeOutState(gameManagerMock.Object);
            state.OnEntry();
            timeProviderMock.Setup(foo => foo.Time).Returns(0);
            for(int i = 0; i < 10; i++) {
                // Just run a bunch to show it's not transitioning
                Assert.IsNull(state.OnDuring()); 
            }
            timeProviderMock.Setup(foo => foo.Time).Returns(FadeOutState.fadeoutTime + 1);
        
            Assert.IsTrue(state.OnDuring() is UpdatePointsState);
        }



    }
}
