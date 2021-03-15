using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DepthFirstTest
    {

        (IGridMap, Stack<Vector2Int>) GenerateMaze()
        {
            var size = new Vector2Int(50, 50);
            var map = new RecursiveBacktracker().GenerateMap(size, 10);
            var path = new DepthFirst().FindPath(new Vector2Int(0, 0), new Vector2Int(49, 49), map);
            Assert.AreNotEqual(path.Count, 0);
            return (map, path);
        }
        [Test]
        public void ShouldFindPathInMinimalCorridor()
        {
            var map = new GridMap(new Vector2Int(2, 1));
            var leftTile = new Tile();
            leftTile.SetWall(Directions.Direction.East, true);
            var rightTile = new Tile();
            leftTile.SetWall(Directions.Direction.West, true);
            map.SetTile(new Vector2Int(0, 0), leftTile);
            map.SetTile(new Vector2Int(1, 0), rightTile);

            var path = (new DepthFirst()).FindPath(new Vector2Int(0, 0), new Vector2Int(1, 0), map);
            Assert.AreEqual(1, path.Count);
            Assert.AreEqual(new Vector2Int(1, 0), path.Pop());
        }

        [Test]
        public void ShouldFindPathInMaze()
        {
            (var map, var path) = GenerateMaze();
            Vector2Int currentPos = Vector2Int.zero;
            while (path.Count > 0)
            {
                currentPos = path.Pop();
            }
            Assert.AreEqual(new Vector2Int(49, 49), currentPos); // We shall end up at goal
        }

        [Test]
        public void FirstStepShouldBeOneAwayFromOrigin()
        {
            (var map, var path) = GenerateMaze();
            var firstPos = path.Pop();
            Assert.AreEqual(1, Mathf.Abs((firstPos.x + firstPos.y)));
        }
        [Test]
        public void EachStepShouldBeOneLong()
        {
            (var map, var path) = GenerateMaze();
            Vector2Int currentPos = Vector2Int.zero;
            while (path.Count > 0)
            {
                var prevPos = currentPos;
                currentPos = path.Pop();
                Assert.AreEqual(1, Mathf.Abs((prevPos.x + prevPos.y) - (currentPos.x + currentPos.y))); // We shall only move one step at a time
            }
        }
    }
}
