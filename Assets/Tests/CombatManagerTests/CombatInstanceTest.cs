using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;

using GameManager;

namespace Tests
{
    public class CombatInstanceTest
    {

        CombatInstance combatInstance;
        Mock<ICharacter> playerMock;
        Mock<IEnemyFactory> enemyFactoryMock;

        List<ICharacter> enemies;
        List<ICharacter> players;
        Mock<IEventRecipient<ICombatUpdateEvent>> eventRecipientMock;

        Mock<ITimeProvider> timeMock;

        Mock<IPlayerWallet> walletMock;
        Mock<ICombatAttributes> mockPlayerAttributes;


        [SetUp]
        public void Setup()
        {
            playerMock = new Mock<ICharacter>();
            players = new List<ICharacter>();
            players.Add(playerMock.Object);
            eventRecipientMock = new Mock<IEventRecipient<ICombatUpdateEvent>>();
            enemies = new List<ICharacter>();
            enemyFactoryMock = new Mock<IEnemyFactory>();
            enemyFactoryMock.Setup(f => f.GenerateEnemies()).Returns(enemies);
            timeMock = new Mock<ITimeProvider>();
            timeMock.Setup(f => f.DeltaTime).Returns(1); // Since turnprogression is DeltaTime*speed this makes calculating it easier
            mockPlayerAttributes = new Mock<ICombatAttributes>();
            playerMock.Setup(f => f.Attributes).Returns(mockPlayerAttributes.Object);
            walletMock = new Mock<IPlayerWallet>();
            combatInstance = new CombatInstance(players.ToArray(), enemyFactoryMock.Object);
        }

        [Test]
        public void IfNoEnemiesShouldBeDone()
        {
            Assert.IsNotNull(combatInstance.Update());
        }

        [Test]
        public void IfPlayerDiesCombatShouldBeDone()
        {
            playerMock.Setup(f => f.IsDead()).Returns(true);
            var enemyMock = new Mock<ICharacter>();
            enemies.Add(enemyMock.Object); // if 0 enemies IsDone returns true
            Assert.IsNotNull(combatInstance.Update());
        }

        [Test]
        public void IfEnemiesDiesShouldBeDone()
        {
            var enemyMock = new Mock<ICharacter>();
            enemyMock.Setup(f => f.IsDead()).Returns(true);
            enemies.Add(enemyMock.Object);
            Assert.IsNotNull(combatInstance.Update());
        }


        [Test]
        public void IfEnemiesDiesButMoreAreLeftShouldNotBeDone()
        {
            {
                var enemyMock = new Mock<ICharacter>();
                enemyMock.Setup(f => f.IsDead()).Returns(true);
                enemies.Add(enemyMock.Object);
            }
            {
                var enemyMock = new Mock<ICharacter>();
                enemyMock.Setup(f => f.IsDead()).Returns(false);
                enemies.Add(enemyMock.Object);
            }
            Assert.IsNull(combatInstance.Update());
        }

        [Test]
        public void PlayersShouldActOnBadGuys()
        {
            var enemyMock = new Mock<ICharacter>();
            enemies.Add(enemyMock.Object);
            combatInstance.Update();
            playerMock.Verify(foo => foo.PerformAction(It.IsAny<List<ICharacter>>(), It.IsAny<ICombatReader>()), Times.Once); // Should only have happened once
            playerMock.Verify(foo => foo.PerformAction(It.Is<List<ICharacter>>(l => l.Equals(enemies)), It.IsAny<ICombatReader>()), Times.Once); // And only on the enemies
        }

        [Test]
        public void BadGuysShouldOnlyActOnPlayers()
        {
            var enemyMock = new Mock<ICharacter>();
            enemies.Add(enemyMock.Object);
            combatInstance.Update();
            enemyMock.Verify(foo => foo.PerformAction(It.IsAny<List<ICharacter>>(), It.IsAny<ICombatReader>()), Times.Once); // Should only have happened once
            enemyMock.Verify(foo => foo.PerformAction(It.Is<List<ICharacter>>(l => l.Contains(players[0])), It.IsAny<ICombatReader>()), Times.Once); // And only on the enemies
        }

        [Test]
        public void PlayerShouldActIfAlive()
        {
            var enemyMock = new Mock<ICharacter>(); // to not finish combat
            enemies.Add(enemyMock.Object);
            combatInstance.Update();
            playerMock.Verify(foo => foo.PerformAction(It.IsAny<List<ICharacter>>(), It.IsAny<ICombatReader>()), Times.Once); // Should only have happened once
        }

        [Test]
        public void PlayerShouldNotActIfDead()
        {
            var enemyMock = new Mock<ICharacter>();
            enemies.Add(enemyMock.Object);
            playerMock.Setup(e => e.IsDead()).Returns(true);
            combatInstance.Update();
            playerMock.Verify(foo => foo.PerformAction(It.IsAny<List<ICharacter>>(), It.IsAny<ICombatReader>()), Times.Never); // Should only have happened once
        }

        [Test]
        public void PlayerWonIfNoEnemiesLeft()
        {
            var result = combatInstance.Update();
            Assert.IsTrue(result.PlayerWon);
        }

        [Test]
        public void PlayerLostIfDeadAndEnemiesLeft()
        {
            var enemyMock = new Mock<ICharacter>();
            enemies.Add(enemyMock.Object);
            players.Clear(); // no players left
            combatInstance = new CombatInstance(players.ToArray(), enemyFactoryMock.Object);
            var result = combatInstance.Update();
            Assert.IsFalse(result.PlayerWon);
        }
    }
}
