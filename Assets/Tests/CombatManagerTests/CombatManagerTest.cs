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
    public class CombatManagerTest
    {
        Mock<IRandomProvider> randomMock;
        Mock<ICombatInstanceFactory> combatFactoryMock;
        Mock<ICombatInstance> combatMock;

        CombatManager manager;


        [SetUp]
        public void Setup()
        {
            randomMock = new Mock<IRandomProvider>();
            combatMock = new Mock<ICombatInstance>();
            combatFactoryMock = new Mock<ICombatInstanceFactory>(MockBehavior.Strict);
            combatFactoryMock.Setup(f => f.CreateInstance(It.IsAny<List<ICombatant>>())).Returns(combatMock.Object);
            CombatManager.ClearInstance();
            manager = new CombatManager(randomMock.Object, combatFactoryMock.Object);
        }

        [Test]
        public void ShouldSpawnCombatInstanceRandomly()
        {
            randomMock.Setup(f => f.ThingHappens(It.IsAny<float>())).Returns(true);
            manager.PlayerEntersTile(Vector2Int.zero);
            combatFactoryMock.Verify(f => f.CreateInstance(It.IsAny<List<ICombatant>>()), Times.AtLeastOnce);

        }

        [Test]
        public void ShouldAlsoNotSpawnCombatInstanceRandomly()
        {
            randomMock.Setup(f => f.ThingHappens(It.IsAny<float>())).Returns(false);
            manager.PlayerEntersTile(Vector2Int.zero);
            combatFactoryMock.Verify(f => f.CreateInstance(It.IsAny<List<ICombatant>>()), Times.Never);
        }

        [Test]
        public void ShouldBeDoneWhenInstanceSaysSo()
        {
            randomMock.Setup(f => f.ThingHappens(It.IsAny<float>())).Returns(true);
            manager.PlayerEntersTile(Vector2Int.zero);
            Assert.IsTrue(manager.InCombat());
            for (int i = 0; i < 100; i++)
            {
                // Spin a while to show that we're not leaving combat
                manager.Update();
                Assert.IsTrue(manager.InCombat());
            }
            combatMock.Setup(f => f.IsDone()).Returns(true);
            manager.Update();
            Assert.IsFalse(manager.InCombat());
        }

        [Test]
        public void ShouldGetEventOnNewCombatInstance()
        {
            CombatManagerUpdateEvent evt = null;
            SimpleObserver<CombatManagerUpdateEvent> observer = new SimpleObserver<CombatManagerUpdateEvent>(manager, (e) => { evt = e; });
            randomMock.Setup(f => f.ThingHappens(It.IsAny<float>())).Returns(true);
            manager.PlayerEntersTile(Vector2Int.zero);
            Assert.IsNotNull(evt);
            Assert.AreEqual(evt.Type, CombatManagerUpdateEvent.UpdateType.EnteredCombat);
            Assert.IsTrue(manager.InCombat());

            observer.Dispose();
        }

        [Test]
        public void UpdateWithoutCombatInstanceShouldDoNothing()
        {
            manager.Update(); // Idk, just don't crash
        }

        [Test]
        public void ShouldGetEventOnCombatDone()
        {
            CombatManagerUpdateEvent evt = null;
            SimpleObserver<CombatManagerUpdateEvent> observer = new SimpleObserver<CombatManagerUpdateEvent>(manager, (e) => { evt = e; });
            randomMock.Setup(f => f.ThingHappens(It.IsAny<float>())).Returns(true);
            manager.PlayerEntersTile(Vector2Int.zero);
            combatMock.Setup(f => f.IsDone()).Returns(true);
            manager.Update();

            Assert.IsNotNull(evt);
            Assert.AreEqual(evt.Type, CombatManagerUpdateEvent.UpdateType.LeftCombat);
            Assert.IsFalse(manager.InCombat());
            observer.Dispose();
        }



    }
}
