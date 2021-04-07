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
    public class DetermineStepStateTest
    {

        Mock<IPlayerMover> playerMoverMock;
        Action onRotateDone;
        Action onMoveDone;
        Mock<ICombatManager> combatManager;

        Mock<IPlayerController> playerMock;

        [SetUp]
        public void Setup()
        {
            combatManager = new Mock<ICombatManager>();
            playerMoverMock = new Mock<IPlayerMover>();
            playerMoverMock.Setup(f => f.MoveTowards(It.IsAny<Vector2Int>(), It.IsAny<Action>())).Callback<Vector2Int, Action>((v, e) => onMoveDone = e);
            playerMoverMock.Setup(f => f.RotateTowards(It.IsAny<Vector2Int>(), It.IsAny<Action>())).Callback<Vector2Int, Action>((v, e)=> onRotateDone = e);
            playerMock = new Mock<IPlayerController>();
        }

        [Test]
        public void ShouldNotifyPathFinishedIfPathDone()
        {
            DetermineStepState state = new DetermineStepState(playerMock.Object);

        }
    }
}
