using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;
using PlayerController;
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
        Mock<ICombatManager> combatManager;
        Stack<Vector2Int> path;

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

            pathfinderMock = new Mock<IPathFinder>();
            path = new Stack<Vector2Int>();
            pathfinderMock.Setup(foo => foo.FindPath(It.IsAny<Vector2Int>(),
                                                    It.IsAny<Vector2Int>(),
                                                    It.IsAny<IMap>())).Returns(path);
            combatManager = new Mock<ICombatManager>();
            playerMoverMock = new Mock<IPlayerMover>();
            playerMoverMock.Setup(f => f.MoveTowards(It.IsAny<Vector2Int>(), It.IsAny<Action>())).Callback<Action>(e => onMoveDone = e);
            playerMoverMock.Setup(f => f.RotateTowards(It.IsAny<Vector2Int>(), It.IsAny<Action>())).Callback<Action>(e => onRotateDone = e);
        }



    }
}
