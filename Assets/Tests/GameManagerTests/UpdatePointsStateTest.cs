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
    public class UpdatePointsStateTest
    {

        Mock<IMap> mapMock;
        Vector2Int mapSize;
 
        Mock<IMapFactory> mapGeneratorMock;
        Mock<IGameManager> gameManagerMock;

        [SetUp]
        public void Setup()
        {
            mapMock = new Mock<IMap>();
            mapSize = new Vector2Int(20, 20);
            mapMock.Setup(foo => foo.Size).Returns(mapSize);

            mapGeneratorMock = new Mock<IMapFactory>();
            mapGeneratorMock.Setup(foo => foo.GenerateMap(It.IsAny<Vector2Int>(), It.IsAny<IRandomProvider>())).Returns(mapMock.Object);

            gameManagerMock = new Mock<IGameManager>();
            gameManagerMock.Setup(foo => foo.MapFactory).Returns(mapGeneratorMock.Object);


        }

        [Test]
        public void ShouldTransitionDirectly()
        {
            var state = new UpdatePointsState(gameManagerMock.Object);
            state.OnEntry();
            Assert.IsTrue(state.OnDuring() is GenerateMapState);
        }
    }
}
