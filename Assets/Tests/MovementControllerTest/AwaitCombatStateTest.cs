using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;

using System;
namespace Tests
{
    public class AwaitCombatStateTest
    {

        Mock<IPlayerMover> playerMoverMock;
        Action onRotateDone;
        Action onMoveDone;
        Mock<ICombatManager> combatManagerMock;

        Mock<IMovementController> playerMock;

        [SetUp]
        public void Setup()
        {
            combatManagerMock = new Mock<ICombatManager>();
            playerMoverMock = new Mock<IPlayerMover>();
            playerMoverMock.Setup(f => f.MoveTowards(It.IsAny<Vector2Int>(), It.IsAny<Action>())).Callback<Vector2Int, Action>((v, e) => onMoveDone = e);
            playerMoverMock.Setup(f => f.RotateTowards(It.IsAny<Vector2Int>(), It.IsAny<Action>())).Callback<Vector2Int, Action>((v, e) => onRotateDone = e);
            playerMock = new Mock<IMovementController>();
            playerMock.Setup(f => f.CombatManager).Returns(combatManagerMock.Object);
        }

        [Test]
        public void ShouldProgressOnEvent()
        {
            var state = new AwaitCombatState(playerMock.Object);
            state.OnEntry();
            for (int i = 0; i < 10; i++)
            {
                Assert.IsNull(state.OnDuring());
            }
            state.HandleEvent(new AwaitCombatState.CombatFinishedEvent());
            Assert.IsTrue(state.OnDuring() is GoToTargetState);
        }
    }
}
