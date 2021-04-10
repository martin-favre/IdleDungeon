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
    public class CombatInstanceTest
    {

        CombatInstance combatInstance;
        Mock<ICombatant> playerMock;
        Mock<IEnemyFactory> enemyFactoryMock;

        List<ICombatant> enemies;
        List<ICombatant> players;
        Mock<IEventRecipient<ICombatUpdateEvent>> eventRecipientMock;

        Mock<ITimeProvider> timeMock;

        Mock<ITurnProgress> turnProgressMock;

        CombatAttributes attributes;

        [SetUp]
        public void Setup()
        {
            playerMock = new Mock<ICombatant>();
            players = new List<ICombatant>();
            players.Add(playerMock.Object);
            eventRecipientMock = new Mock<IEventRecipient<ICombatUpdateEvent>>();
            enemies = new List<ICombatant>();
            enemyFactoryMock = new Mock<IEnemyFactory>();
            enemyFactoryMock.Setup(f => f.GenerateEnemies()).Returns(enemies);
            timeMock = new Mock<ITimeProvider>();
            timeMock.Setup(f => f.DeltaTime).Returns(1); // Since turnprogression is DeltaTime*speed this makes calculating it easier
            turnProgressMock = new Mock<ITurnProgress>();
            turnProgressMock.Setup(f => f.IncrementTurnProgress(It.IsAny<float>())).Returns(true); // By default it's always everyone's turn
            playerMock.Setup(f => f.TurnProgress).Returns(turnProgressMock.Object);
            attributes = new CombatAttributes();
            attributes.Speed = (int)TurnProgress.MaxTurnProgress;
            playerMock.Setup(f => f.Attributes).Returns(attributes);
            combatInstance = new CombatInstance(players.ToArray(), enemyFactoryMock.Object, eventRecipientMock.Object, timeMock.Object);
        }

        [Test]
        public void IfNoEnemiesShouldBeDone()
        {
            Assert.IsTrue(combatInstance.IsDone());
        }

        [Test]
        public void IfPlayerDiesCombatShouldBeDone()
        {
            playerMock.Setup(f => f.IsDead()).Returns(true);
            var enemyMock = new Mock<ICombatant>();
            enemies.Add(enemyMock.Object); // if 0 enemies IsDone returns true
            Assert.IsFalse(combatInstance.IsDone()); // reality check
            combatInstance.Update();
            Assert.IsTrue(combatInstance.IsDone());
        }

        [Test]
        public void IfEnemiesDiesShouldBeDone()
        {
            var enemyMock = new Mock<ICombatant>();
            enemyMock.Setup(f => f.IsDead()).Returns(true);
            enemies.Add(enemyMock.Object);
            Assert.IsFalse(combatInstance.IsDone());
            combatInstance.Update();
            Assert.IsTrue(combatInstance.IsDone());
        }


        [Test]
        public void IfEnemiesDiesButMoreAreLeftShouldNotBeDone()
        {
            {
                var enemyMock = new Mock<ICombatant>();
                enemyMock.Setup(f => f.IsDead()).Returns(true);
                enemies.Add(enemyMock.Object);
            }
            {
                var enemyMock = new Mock<ICombatant>();
                enemyMock.Setup(f => f.IsDead()).Returns(false);
                enemies.Add(enemyMock.Object);
            }

            Assert.IsFalse(combatInstance.IsDone());
            combatInstance.Update();
            Assert.IsFalse(combatInstance.IsDone());
        }

        [Test]
        public void PlayersShouldActOnBadGuys()
        {
            var enemyMock = new Mock<ICombatant>();
            enemies.Add(enemyMock.Object);
            combatInstance.Update();
            playerMock.Verify(foo => foo.PerformAction(It.IsAny<List<ICombatant>>(), It.IsAny<ICombatReader>(),
                It.Is<IEventRecipient<ICombatUpdateEvent>>(e => e == eventRecipientMock.Object)), Times.Once); // Should only have happened once
            playerMock.Verify(foo => foo.PerformAction(It.Is<List<ICombatant>>(l => l.Equals(enemies)), It.IsAny<ICombatReader>(),
                It.Is<IEventRecipient<ICombatUpdateEvent>>(e => e == eventRecipientMock.Object)), Times.Once); // And only on the enemies
        }

        [Test]
        public void BadGuysShouldOnlyActOnPlayers()
        {
            var enemyMock = new Mock<ICombatant>();
            enemies.Add(enemyMock.Object);
            combatInstance.Update();
            enemyMock.Verify(foo => foo.PerformAction(It.IsAny<List<ICombatant>>(), It.IsAny<ICombatReader>(),
                It.Is<IEventRecipient<ICombatUpdateEvent>>(e => e == eventRecipientMock.Object)), Times.Once); // Should only have happened once
            enemyMock.Verify(foo => foo.PerformAction(It.Is<List<ICombatant>>(l => l.Contains(players[0])), It.IsAny<ICombatReader>(),
                It.Is<IEventRecipient<ICombatUpdateEvent>>(e => e == eventRecipientMock.Object)), Times.Once); // And only on the enemies
        }

        [Test]
        public void PlayerShouldActIfTheirTurn()
        {
            var enemyMock = new Mock<ICombatant>();
            enemies.Add(enemyMock.Object);

            turnProgressMock.Setup(f => f.IncrementTurnProgress(It.IsAny<float>())).Returns(true);
            combatInstance.Update();
            playerMock.Verify(foo => foo.PerformAction(It.IsAny<List<ICombatant>>(), It.IsAny<ICombatReader>(),
                It.Is<IEventRecipient<ICombatUpdateEvent>>(e => e == eventRecipientMock.Object)), Times.Once); // Should only have happened once
        }
        [Test]
        public void PlayerShouldNotActIfNotTheirTurn()
        {
            var enemyMock = new Mock<ICombatant>();
            enemies.Add(enemyMock.Object);

            turnProgressMock.Setup(f => f.IncrementTurnProgress(It.IsAny<float>())).Returns(false);
            combatInstance.Update();
            playerMock.Verify(foo => foo.PerformAction(It.IsAny<List<ICombatant>>(), It.IsAny<ICombatReader>(),
                It.Is<IEventRecipient<ICombatUpdateEvent>>(e => e == eventRecipientMock.Object)), Times.Never); // Should only have happened once
        }

    }
}
