using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;
using System;
namespace Tests
{
    public class GoalAndStartAdderTest
    {
        Mock<IMap> mapMock;
        Vector2Int mapSize;
        Tile openTile;
        Tile closedTile;
        Mock<IRandomProvider> randomMock;

        [SetUp]
        public void Setup()
        {
            mapMock = new Mock<IMap>();
            mapSize = new Vector2Int(20, 20);
            mapMock.Setup(foo => foo.Size).Returns(mapSize);
            mapMock.Setup(foo => foo.InsideMap(It.IsAny<Vector2Int>())).Returns(true);
            openTile = new Tile();
            openTile.SetAllWalls(true);
            closedTile = new Tile();
            closedTile.SetAllWalls(false);
            randomMock = new Mock<IRandomProvider>();

        }

        void SetUpSuccessfulSequence()
        {

            // I don't know if this is so robust
            // Bu it should return a sequence which will make the algorithm set a goal and a start
            mapMock.SetupSequence(foo => foo.GetTile(It.IsAny<Vector2Int>())).
                Returns(openTile).Returns(closedTile).Returns(closedTile).Returns(closedTile).
                Returns(openTile).Returns(closedTile).Returns(closedTile).Returns(closedTile);

        }

        [Test]
        public void GoalAndEndShouldBeSet()
        {
            SetUpSuccessfulSequence();
            var adder = new GoalAndStartAdder();
            adder.ImproveMap(mapMock.Object, randomMock.Object);
            mapMock.VerifySet(foo => foo.Goal = It.IsAny<Vector2Int>());
            mapMock.VerifySet(foo => foo.Start = It.IsAny<Vector2Int>());
        }


        [Test]
        public void GoalAndEndShouldHaveCorrectTiles()
        {
            SetUpSuccessfulSequence();
            var adder = new GoalAndStartAdder();
            adder.ImproveMap(mapMock.Object, randomMock.Object);
            mapMock.Verify(foo => foo.SetTile(It.IsAny<Vector2Int>(), It.IsAny<GoalTile>()));
            mapMock.Verify(foo => foo.SetTile(It.IsAny<Vector2Int>(), It.IsAny<StartTile>()));
        }

        [Test]
        public void GoalAndEndShouldNotBeInSameLocation()
        {
            SetUpSuccessfulSequence();
            var startLocation = new Vector2Int(int.MaxValue, int.MaxValue);
            var goalLocation = new Vector2Int(int.MaxValue, int.MaxValue);
            mapMock.SetupSet(h => h.Start = It.IsAny<Vector2Int>()).Callback<Vector2Int>(r => startLocation = r);
            mapMock.SetupSet(h => h.Goal = It.IsAny<Vector2Int>()).Callback<Vector2Int>(r => goalLocation = r);
            var adder = new GoalAndStartAdder();
            adder.ImproveMap(mapMock.Object, randomMock.Object);

            Assert.AreNotEqual(startLocation, goalLocation);

        }

    }
}
