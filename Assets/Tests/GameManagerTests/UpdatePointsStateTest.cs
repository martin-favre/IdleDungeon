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
        Mock<IPersistentDataStorage> persistentStorageMock;

        Mock<IPlayerCharacters> playerCharsMock;

        [SetUp]
        public void Setup()
        {
            mapMock = new Mock<IMap>();
            mapSize = new Vector2Int(20, 20);
            mapMock.Setup(foo => foo.Size).Returns(mapSize);

            mapGeneratorMock = new Mock<IMapFactory>();
            mapGeneratorMock.Setup(foo => foo.GenerateMap(It.IsAny<Vector2Int>(), It.IsAny<IRandomProvider>())).Returns(mapMock.Object);

            persistentStorageMock = new Mock<IPersistentDataStorage>();
            gameManagerMock = new Mock<IGameManager>();
            playerCharsMock = new Mock<IPlayerCharacters>();
            gameManagerMock.Setup(foo => foo.MapFactory).Returns(mapGeneratorMock.Object);
            gameManagerMock.Setup(foo => foo.DataStorage).Returns(persistentStorageMock.Object);
            gameManagerMock.Setup(foo => foo.PlayerChars).Returns(playerCharsMock.Object);
            
        }

        [Test]
        public void ShouldTransitionDirectly()
        {
            var state = new UpdatePointsState(gameManagerMock.Object, false);
            state.OnEntry();
            Assert.IsTrue(state.OnDuring() is GenerateMapState);
        }

        [Test]
        public void ShouldIncrementLevelByOneIfCompletedMap()
        {
            persistentStorageMock.Setup(foo => foo.GetInt(It.Is<string>((s) => s.Equals(Constants.currentLevelKey)), It.IsAny<int>())).Returns(5);
            
            var state = new UpdatePointsState(gameManagerMock.Object, false);
            state.OnEntry();
            persistentStorageMock.Verify(foo => foo.SetInt(It.Is<string>((s) => s.Equals(Constants.currentLevelKey)), It.Is<int>(i => i == 6)));
        }
        [Test]
        public void ShouldDecrementLevelByOneIfPlayerDied()
        {
            persistentStorageMock.Setup(foo => foo.GetInt(It.Is<string>((s) => s.Equals(Constants.currentLevelKey)), It.IsAny<int>())).Returns(5);
            
            var state = new UpdatePointsState(gameManagerMock.Object, true);
            state.OnEntry();
            persistentStorageMock.Verify(foo => foo.SetInt(It.Is<string>((s) => s.Equals(Constants.currentLevelKey)), It.Is<int>(i => i == 4)));
        }

        [Test]
        public void ShouldStayOnLevel0IfPlayerDied()
        {
            persistentStorageMock.Setup(foo => foo.GetInt(It.Is<string>((s) => s.Equals(Constants.currentLevelKey)), It.IsAny<int>())).Returns(0);
            
            var state = new UpdatePointsState(gameManagerMock.Object, true);
            state.OnEntry();
            persistentStorageMock.Verify(foo => foo.SetInt(It.Is<string>((s) => s.Equals(Constants.currentLevelKey)), It.Is<int>(i => i == 0)));
        }


        [Test]
        public void LevelShouldDefaultToZero()
        {   

            var state = new UpdatePointsState(gameManagerMock.Object, false);
            state.OnEntry();
            persistentStorageMock.Verify(foo => foo.GetInt(It.Is<string>((s) => s.Equals(Constants.currentLevelKey)), It.Is<int>(i => i == 0)));
            // It defaults to 0, so when we call OnEntry, it becomes 1
            persistentStorageMock.Verify(foo => foo.SetInt(It.Is<string>((s) => s.Equals(Constants.currentLevelKey)), It.Is<int>(i => i == 1)));
        
        }

    }
}
