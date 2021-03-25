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
    public class GenerateMapStateTest
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
        public void ShouldGenerateMap()
        {
            var state = new GenerateMapState(gameManagerMock.Object);
            state.OnEntry();
            state.OnDuring();
            gameManagerMock.VerifySet(foo => foo.GridMap = mapMock.Object);
        }

        [Test]
        public void ShouldReturnPlayGameState()
        {
            var state = new GenerateMapState(gameManagerMock.Object);
            state.OnEntry();
            Assert.IsTrue(state.OnDuring() is PlayGameState);
            Assert.IsFalse(state.MachineTerminated);
        }




    }
}
