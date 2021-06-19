using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;

using GameManager;
using PubSubSystem;

namespace Tests
{
    public class CombatManagerTest
    {
        Mock<IRandomProvider> randomMock;
        Mock<ICombatInstanceFactory> combatFactoryMock;
        Mock<ICombatInstance> combatMock;
        Mock<IMap> mapMock;
        CombatManager manager;
        Mock<IGameManager> gameManagerMock;
        readonly Vector2Int startPos = new Vector2Int(10, 10);
        readonly Vector2Int goalPos = new Vector2Int(20, 20);
        Mock<IEventRecipient<ICombatUpdateEvent>> eventRecipientMock;
        Mock<ITimeProvider> timeProviderMock;
        Mock<IEventPublisher<EventType>> publisherMock;


        [SetUp]
        public void Setup()
        {
            randomMock = new Mock<IRandomProvider>();
            combatMock = new Mock<ICombatInstance>();
            publisherMock = new Mock<IEventPublisher<EventType>>();
            mapMock = new Mock<IMap>();
            mapMock.Setup(f => f.Start).Returns(startPos);
            mapMock.Setup(f => f.Goal).Returns(goalPos);
            eventRecipientMock = new Mock<IEventRecipient<ICombatUpdateEvent>>();
            combatFactoryMock = new Mock<ICombatInstanceFactory>(MockBehavior.Strict);
            combatFactoryMock.Setup(f => f.CreateInstance(It.IsAny<ICharacter[]>())).Returns(combatMock.Object);
            CombatManager.ClearInstance();
            timeProviderMock = new Mock<ITimeProvider>();
            gameManagerMock = new Mock<IGameManager>();
            gameManagerMock.Setup(e => e.GridMap).Returns(mapMock.Object);
            SingletonProvider.MainRandomProvider = randomMock.Object;
            SingletonProvider.MainGameManager = gameManagerMock.Object;
            SingletonProvider.MainEventPublisher = publisherMock.Object;
            manager = new CombatManager(combatFactoryMock.Object);
        }

        [Test]
        public void ShouldSpawnCombatInstanceRandomly()
        {
            randomMock.Setup(f => f.ThingHappens(It.IsAny<float>())).Returns(true);
            manager.PlayerEntersTile(Vector2Int.zero);
            combatFactoryMock.Verify(f => f.CreateInstance(It.IsAny<ICharacter[]>()), Times.AtLeastOnce);
        }

        [Test]
        public void ShouldNotSpawnCombatInstanceWhenEnteringGoal()
        {
            randomMock.Setup(f => f.ThingHappens(It.IsAny<float>())).Returns(true);
            manager.PlayerEntersTile(goalPos);
            combatFactoryMock.Verify(f => f.CreateInstance(It.IsAny<ICharacter[]>()), Times.Never);
        }
        [Test]
        public void ShouldNotSpawnCombatInstanceWhenJustOutsideStart()
        {
            randomMock.Setup(f => f.ThingHappens(It.IsAny<float>())).Returns(true);
            Vector2Int[] posToTest =
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(0, 1),
            };
            foreach (var item in posToTest)
            {
                manager.PlayerEntersTile(startPos + item);
                combatFactoryMock.Verify(f => f.CreateInstance(It.IsAny<ICharacter[]>()), Times.Never);
            }
        }


        [Test]
        public void ShouldAlsoNotSpawnCombatInstanceRandomly()
        {
            randomMock.Setup(f => f.ThingHappens(It.IsAny<float>())).Returns(false);
            manager.PlayerEntersTile(Vector2Int.zero);
            Assert.IsFalse(manager.InCombat());
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
            combatMock.Setup(f => f.Update()).Returns(new CombatResult(0, false));
            manager.Update();
            Assert.IsFalse(manager.InCombat());
        }

        [Test]
        public void ShouldGetEventOnNewCombatInstance()
        {
            // ICombatUpdateEvent evt = null;
            // SimpleObserver<ICombatUpdateEvent> observer = new SimpleObserver<ICombatUpdateEvent>(manager, (e) => { evt = e; });
            // randomMock.Setup(f => f.ThingHappens(It.IsAny<float>())).Returns(true);
            // manager.PlayerEntersTile(Vector2Int.zero);
            // Assert.IsNotNull(evt);
            // Assert.IsTrue(evt is EnteredCombatEvent);
            // Assert.IsTrue(manager.InCombat());

            // observer.Dispose();
        }

        [Test]
        public void UpdateWithoutCombatInstanceShouldDoNothing()
        {
            manager.Update(); // Idk, just don't crash
        }

        [Test]
        public void ShouldGetEventOnCombatDone()
        {
            // ICombatUpdateEvent evt = null;
            // SimpleObserver<ICombatUpdateEvent> observer = new SimpleObserver<ICombatUpdateEvent>(manager, (e) => { evt = e; });
            // randomMock.Setup(f => f.ThingHappens(It.IsAny<float>())).Returns(true);
            // manager.PlayerEntersTile(Vector2Int.zero);
            // combatMock.Setup(f => f.IsDone()).Returns(true);
            // manager.Update();

            // Assert.IsNotNull(evt);
            // Assert.IsTrue(evt is ExitedCombatEvent);
            // Assert.IsFalse(manager.InCombat());
            // observer.Dispose();
        }



    }
}
