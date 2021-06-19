using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;

using System;
using PubSubSystem;

namespace Tests
{
    public class MovementControllerTest
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
        MovementController controller;

        Mock<ICombatReader> combatReaderMock;

        Mock<GameManager.PlayerCallbacks> callbacksMock;

        Mock<IEventPublisher<EventType>> mockPublisher;

        Action<IEvent> publishCallback;

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
            mockPublisher = new Mock<IEventPublisher<EventType>>();
            mockPublisher.Setup(e => e.Subscribe(It.IsAny<EventType[]>(),
                It.IsAny<Action<IEvent>>())).
                Callback<EventType[], Action<IEvent>>((type, action) => publishCallback = action);
            playerMoverMock = new Mock<IPlayerMover>();
            SingletonProvider.MainEventHandler = mockPublisher.Object;
            callbacksMock = new Mock<GameManager.PlayerCallbacks>();
            controller = new MovementController(mapMock.Object,
                                                pathfinderMock.Object,
                                                callbacksMock.Object,
                                                combatManagerMock.Object,
                                                playerMoverMock.Object);

        }

        [Test]
        public void PlayerShouldSubscribe()
        {
            Assert.IsNotNull(publishCallback);
        }

        [Test]
        public void CallPlayerDiedOnExitCombatWherePlayerDied()
        {
            publishCallback(new CombatResultsClosedEvent(new CombatResult(0, false)));
            callbacksMock.Verify(f => f.OnPlayerDied());
        }

        [Test]
        public void CallNothingOnEnteredCombatEvent()
        {
            publishCallback(new CombatStartedEvent(combatReaderMock.Object));
            callbacksMock.Verify(f => f.OnPlayerDied(), Times.Never);
        }

        [Test]
        public void CallNothingOnExitedCombatEventWherePlayerSurvived()
        {
            publishCallback(new CombatEndedEvent(null, new CombatResult(0, true)));
            callbacksMock.Verify(f => f.OnPlayerDied(), Times.Never);
        }

    }
}
