using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DepthFirstTest
    {
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


    }
}
