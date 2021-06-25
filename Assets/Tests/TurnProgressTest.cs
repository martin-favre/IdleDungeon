using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;

using GameManager;

namespace Tests
{
    public class TurnProgressTest
    {
        TurnProgress progress;
        const float defaultActionTime = 123;
        Mock<ITimeProvider> timeProviderMock;

        [SetUp]
        public void Setup()
        {
            timeProviderMock = new Mock<ITimeProvider>();
            SingletonProvider.MainTimeProvider = timeProviderMock.Object;

            progress = new TurnProgress(defaultActionTime);
        }

        [Test]
        public void IsDone_ReturnFalseIfNotTimedOut()
        {
            timeProviderMock.Setup(f => f.Time).Returns(0);
            Assert.False(progress.IsDone());
        }
        [Test]
        public void IsDone_ReturnTrueIfTimedOut()
        {
            timeProviderMock.Setup(f => f.Time).Returns(defaultActionTime + 1);
            Assert.True(progress.IsDone());
        }
        [Test]
        public void GetRelativeProgress_ReturnZeroIfNoProgress()
        {
            timeProviderMock.Setup(f => f.Time).Returns(0);
            Assert.AreEqual(progress.GetRelativeProgress(), 0, 0.01f);
        }
        [Test]
        public void GetRelativeProgress_ReturnHalfIfSomeProgress()
        {
            timeProviderMock.Setup(f => f.Time).Returns(defaultActionTime / 2);
            Assert.AreEqual(progress.GetRelativeProgress(), 0.5f, 0.01f);
        }
        [Test]
        public void GetRelativeProgress_ReturnFullIfFullProgress()
        {
            timeProviderMock.Setup(f => f.Time).Returns(defaultActionTime);
            Assert.AreEqual(progress.GetRelativeProgress(), 1f, 0.01f);
        }
        [Test]
        public void GetRelativeProgress_ReturnFullIfMoreThanFullProgress()
        {
            timeProviderMock.Setup(f => f.Time).Returns(defaultActionTime * 2);
            Assert.AreEqual(progress.GetRelativeProgress(), 1f, 0.01f);
        }

        [Test]
        public void Reset_ProgressShouldBeReset()
        {
            timeProviderMock.Setup(f => f.Time).Returns(defaultActionTime);
            Assert.AreEqual(progress.GetRelativeProgress(), 1, 0.01f); // reality check
            progress.Reset();
            Assert.AreEqual(progress.GetRelativeProgress(), 0, 0.01f); // reality check
        }
        [Test]
        public void Reset_IsDoneShouldBeReset()
        {
            timeProviderMock.Setup(f => f.Time).Returns(defaultActionTime + 1);
            Assert.True(progress.IsDone());
            progress.Reset();
            Assert.False(progress.IsDone());
        }


    }
}
