using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;

using System;
namespace Tests
{
    public class PlayerControllerTest
    {

        Mock<ITimeProvider> timeMock;
        Mock<IPlayerMover> playerMoverMock;
        Action onRotateDone;
        Action onMoveDone;
        float currentTime;
        Mock<IMap> mapMock;
        Vector2Int mapSize;
        Tile mockTile;

        Mock<IPathFinder> pathfinderMock;
        Mock<ICombatManager> combatManagerMock;
        Stack<Vector2Int> path;
        PlayerController controller;

        Mock<ICombatReader> combatReaderMock;

        Mock<GameManager.PlayerCallbacks> callbacksMock;

        IObserver<ICombatUpdateEvent> playersObserver;

        [SetUp]
        public void Setup()
        {
            timeMock = new Mock<ITimeProvider>();
            currentTime = 0;
            timeMock.Setup(foo => foo.Time).Returns(() => currentTime);
            mapMock = new Mock<IMap>();
            mapSize = new Vector2Int(20, 20);
            mapMock.Setup(foo => foo.Size).Returns(mapSize);
            mockTile = new Tile();
            mockTile.SetAllWalls(true);
            mapMock.Setup(foo => foo.GetTile(It.IsAny<Vector2Int>())).Returns(mockTile);
            combatReaderMock = new Mock<ICombatReader>();
            pathfinderMock = new Mock<IPathFinder>();
            path = new Stack<Vector2Int>();
            pathfinderMock.Setup(foo => foo.FindPath(It.IsAny<Vector2Int>(),
                                                    It.IsAny<Vector2Int>(),
                                                    It.IsAny<IMap>())).Returns(path);
            combatManagerMock = new Mock<ICombatManager>();
            combatManagerMock.Setup(f => f.Subscribe(It.IsAny<IObserver<ICombatUpdateEvent>>())).Callback<IObserver<ICombatUpdateEvent>>(e => playersObserver = e);
            playerMoverMock = new Mock<IPlayerMover>();
            callbacksMock = new Mock<GameManager.PlayerCallbacks>();
            controller = new PlayerController(mapMock.Object,
                                                pathfinderMock.Object,
                                                callbacksMock.Object,
                                                combatManagerMock.Object,
                                                playerMoverMock.Object);
        }

        [Test]
        public void PlayerOughtToRegisterAnObserver()
        {
            // mostly a realitycheck
            Assert.IsNotNull(playersObserver);
        }

        [Test]
        public void CallPlayerDiedOnExitCombatWherePlayerDied()
        {
            playersObserver.OnNext(new ExitedCombatEvent(null, ExitedCombatEvent.CombatResult.PlayerLost));
            callbacksMock.Verify(f => f.OnPlayerDied());
        }

        [Test]
        public void CallNothingOnEnteredCombatEvent()
        {
            playersObserver.OnNext(new EnteredCombatEvent(combatReaderMock.Object));
            callbacksMock.Verify(f => f.OnPlayerDied(), Times.Never);
        }

        [Test]
        public void CallNothingOnExitedCombatEventWherePlayerSurvived()
        {
            playersObserver.OnNext(new ExitedCombatEvent(null, ExitedCombatEvent.CombatResult.PlayerWon));
            callbacksMock.Verify(f => f.OnPlayerDied(), Times.Never);
        }

    }
}
