using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;

using System;
namespace Tests
{
    public class DetermineStepStateTest
    {

        Mock<IPlayerMover> playerMoverMock;
        Action onRotateDone;
        Action onMoveDone;
        Mock<ICombatManager> combatManagerMock;

        Mock<IPlayerController> playerMock;

        [SetUp]
        public void Setup()
        {
            combatManagerMock = new Mock<ICombatManager>();
            playerMoverMock = new Mock<IPlayerMover>();
            playerMoverMock.Setup(f => f.MoveTowards(It.IsAny<Vector2Int>(), It.IsAny<Action>())).Callback<Vector2Int, Action>((v, e) => onMoveDone = e);
            playerMoverMock.Setup(f => f.RotateTowards(It.IsAny<Vector2Int>(), It.IsAny<Action>())).Callback<Vector2Int, Action>((v, e) => onRotateDone = e);
            playerMock = new Mock<IPlayerController>();
            playerMock.Setup(f => f.CombatManager).Returns(combatManagerMock.Object);
        }

        [Test]
        public void ShouldNotifyPathFinishedIfPathDone()
        {
            DetermineStepState state = new DetermineStepState(playerMock.Object);
            state.OnEntry();
            state.OnDuring();
            playerMock.Verify(f => f.NotifyPathFinished(), Times.Once);
        }

        [Test]
        public void ShouldRequestToLookAtNextPos()
        {
            playerMock.Setup(f => f.HasNextStep()).Returns(true);
            playerMock.Setup(f => f.GetNextStep()).Returns(new Vector2Int(15, 13));
            DetermineStepState state = new DetermineStepState(playerMock.Object);
            state.OnEntry();
            state.OnDuring();
            playerMock.Verify(f => f.RequestLookAt(It.Is<Vector2Int>(v2 => v2 == new Vector2Int(15, 13))), Times.Once);
        }

        [Test]
        public void ShouldProgressOnEvent()
        {
            playerMock.Setup(f => f.HasNextStep()).Returns(true);
            playerMock.Setup(f => f.GetNextStep()).Returns(new Vector2Int(15, 13));
            DetermineStepState state = new DetermineStepState(playerMock.Object);
            state.OnEntry();
            Assert.IsNull(state.OnDuring());

            for (int i = 0; i < 10; i++)
            {
                // spin to show we're not changing
                Assert.IsNull(state.OnDuring());
            }

            state.HandleEvent(new DetermineStepState.TurningFinishedEvent());
            Assert.IsNotNull(state.OnDuring());
        }
        [Test]
        public void GoToAwaitCombatStateIfCombatStarted()
        {
            playerMock.Setup(f => f.HasNextStep()).Returns(true);
            playerMock.Setup(f => f.GetNextStep()).Returns(new Vector2Int(15, 13));
            playerMock.Setup(f => f.GridPosition).Returns(new Vector2Int(15, 13));
            combatManagerMock.Setup(f => f.PlayerEntersTile(It.Is<Vector2Int>(v2 => v2 == new Vector2Int(15, 13)))).Returns(true);
            DetermineStepState state = new DetermineStepState(playerMock.Object);
            state.OnEntry();
            state.HandleEvent(new DetermineStepState.TurningFinishedEvent());
            Assert.IsTrue(state.OnDuring() is AwaitCombatState);
        }
        [Test]
        public void GoToTargetStateIfNotCombatStarted()
        {
            playerMock.Setup(f => f.HasNextStep()).Returns(true);
            playerMock.Setup(f => f.GetNextStep()).Returns(new Vector2Int(15, 13));
            playerMock.Setup(f => f.GridPosition).Returns(new Vector2Int(15, 13));
            combatManagerMock.Setup(f => f.PlayerEntersTile(It.Is<Vector2Int>(v2 => v2 == new Vector2Int(15, 13)))).Returns(false);
            DetermineStepState state = new DetermineStepState(playerMock.Object);
            state.OnEntry();
            state.HandleEvent(new DetermineStepState.TurningFinishedEvent());
            Assert.IsTrue(state.OnDuring() is GoToTargetState);
        }

        [Test]
        public void ShouldBeAbleToThrowEventStraightAway()
        {
            playerMock.Setup(f => f.HasNextStep()).Returns(true);
            playerMock.Setup(f => f.GetNextStep()).Returns(new Vector2Int(15, 13));
            playerMock.Setup(f => f.GridPosition).Returns(new Vector2Int(15, 13));
            combatManagerMock.Setup(f => f.PlayerEntersTile(It.Is<Vector2Int>(v2 => v2 == new Vector2Int(15, 13)))).Returns(false);
            DetermineStepState state = new DetermineStepState(playerMock.Object);
            playerMock.Setup(f => f.RequestLookAt(It.IsAny<Vector2Int>())).Callback<Vector2Int>(v2 => state.HandleEvent(new DetermineStepState.TurningFinishedEvent()));
            state.OnEntry();

            Assert.IsTrue(state.OnDuring() is GoToTargetState);
        }

    }
}
