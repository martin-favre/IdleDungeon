using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;
using PubSubSystem;
using System;

namespace Tests
{
    public class PlayerCharacterTests
    {
        PlayerCharacter playerCharacter;
        Mock<IEventPublisher<EventType>> eventPublisherMock;
        Action<IEvent> publishCallback;
        Mock<ICharacter> enemyMock;
        Mock<ICharacterAction> actionMock;
        Mock<ICombatReader> combatReaderMock;
        Mock<ICombatManager> combatManagerMock;
        Mock<ICombatAttributes> combatAttributesMock;

        [SetUp]
        public void Setup()
        {
            eventPublisherMock = new Mock<IEventPublisher<EventType>>();
            eventPublisherMock.Setup(e => e.Subscribe(
                It.Is<EventType>(type => type == EventType.PlayerSelectedActionTarget),
                It.IsAny<Action<IEvent>>())).
                Callback<EventType,
                Action<IEvent>>((type, action) => publishCallback = action);

            combatReaderMock = new Mock<ICombatReader>();
            combatManagerMock = new Mock<ICombatManager>();
            combatManagerMock.Setup(f => f.CombatReader).Returns(combatReaderMock.Object);
            SingletonProvider.MainEventPublisher = eventPublisherMock.Object;
            SingletonProvider.MainCombatManager = combatManagerMock.Object;
            enemyMock = new Mock<ICharacter>();
            actionMock = new Mock<ICharacterAction>();
            combatAttributesMock = new Mock<ICombatAttributes>();
            playerCharacter = new PlayerCharacter(0, 10, combatAttributesMock.Object, new[] { actionMock.Object });
        }

        [Test]
        public void PlayerShouldSubscribe()
        {
            // reality check as other tests bases on this
            Assert.IsNotNull(publishCallback);
        }

        [Test]
        public void PlayerShouldStartChargingAction()
        {
            publishCallback(new PlayerSelectedActionTargetEvent(playerCharacter, enemyMock.Object, actionMock.Object));
            actionMock.Verify(f => f.StartChargingAction(
                It.Is<ICharacter>(chr => chr == playerCharacter),
                It.Is<ICharacter>(chr => chr == enemyMock.Object),
                It.Is<ICombatReader>(r => r == combatReaderMock.Object)
                ));
        }
        [Test]
        public void PlayerShouldNotChargeIfNotTheirAction()
        {
            publishCallback(new PlayerSelectedActionTargetEvent(enemyMock.Object, playerCharacter, actionMock.Object));
            actionMock.Verify(f => f.StartChargingAction(
                It.IsAny<ICharacter>(),
                It.IsAny<ICharacter>(),
                It.IsAny<ICombatReader>()
                ), Times.Never);
        }
    }
}
