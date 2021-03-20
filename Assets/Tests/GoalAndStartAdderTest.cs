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
        Tile mockTile;


        [SetUp]
        public void Setup()
        {
            mapMock = new Mock<IMap>();
            mapSize = new Vector2Int(20, 20);
            mapMock.Setup(foo => foo.Size).Returns(mapSize);
            mockTile = new Tile();
            mockTile.SetAllWalls(true);


        }

        // Tests disabled for now
        // I don't know how to mock GridMap
        // So it can reliably return a specific pattern of tiles

        // [Test]
        // public void GoalAndEndShouldBeSet()
        // {
        //     mapMock.Setup(foo => foo.GetTile(It.IsAny<Vector2Int>())).Returns(mockTile);
        //     var adder = new GoalAndStartAdder();
        //     adder.ImproveMap(mapMock.Object, 10);
        //     mapMock.VerifySet(foo => foo.Goal = It.IsAny<Vector2Int>());
        //     mapMock.VerifySet(foo => foo.Start = It.IsAny<Vector2Int>());
        // }

        // [Test]
        // public void GoalAndEndShouldBeSetEvenIfNotFirstTileIsFree()
        // {
        //     var blockedTile = new Tile();
        //     blockedTile.SetAllWalls(false);
        //     var openTile = new Tile();
        //     openTile.SetAllWalls(true);
        //     mapMock.SetupSequence(foo => foo.GetTile(It.IsAny<Vector2Int>()))
        //         .Returns(blockedTile).Returns(blockedTile).Returns(openTile) // for goal
        //         .Returns(blockedTile).Returns(blockedTile).Returns(openTile); // for start
        //     var adder = new GoalAndStartAdder();
        //     adder.ImproveMap(mapMock.Object, 10);
        //     mapMock.VerifySet(foo => foo.Goal = It.IsAny<Vector2Int>());
        //     mapMock.VerifySet(foo => foo.Start = It.IsAny<Vector2Int>());
        // }



        // [Test]
        // public void GoalAndEndShouldHaveCorrectTiles()
        // {
        //     mapMock.Setup(foo => foo.GetTile(It.IsAny<Vector2Int>())).Returns(mockTile);
        //     var adder = new GoalAndStartAdder();
        //     adder.ImproveMap(mapMock.Object, 10);
        //     mapMock.Verify(foo => foo.SetTile(It.IsAny<Vector2Int>(), It.IsAny<GoalTile>()));
        //     mapMock.Verify(foo => foo.SetTile(It.IsAny<Vector2Int>(), It.IsAny<StartTile>()));
        // }

        // [Test]
        // public void GoalAndEndShouldNotBeInSameLocation()
        // {
        //     mapMock.Setup(foo => foo.GetTile(It.IsAny<Vector2Int>())).Returns(mockTile);
        //     var startLocation = new Vector2Int(int.MaxValue, int.MaxValue);
        //     var goalLocation = new Vector2Int(int.MaxValue, int.MaxValue);
        //     mapMock.SetupSet(h => h.Start = It.IsAny<Vector2Int>()).Callback<Vector2Int>(r => startLocation = r);
        //     mapMock.SetupSet(h => h.Goal = It.IsAny<Vector2Int>()).Callback<Vector2Int>(r => goalLocation = r);
        //     var adder = new GoalAndStartAdder();
        //     adder.ImproveMap(mapMock.Object, 10);
            
        //     Assert.AreNotEqual(startLocation, goalLocation);

        // }

    }
}
