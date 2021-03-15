using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TileTest
    {
        [Test]
        public void ShouldBeAbleToCopyTile()
        {
            Tile tile = new Tile();
            tile.SetWall(Directions.Direction.North, true);
            Tile otherTile = new Tile(tile);
            Assert.AreEqual(otherTile.GetWall(Directions.Direction.North), true);
        }
        [Test]
        public void CopiesShouldBeDeep()
        {
            Tile tile = new Tile();
            tile.SetWall(Directions.Direction.North, true);
            Tile otherTile = new Tile(tile);
            otherTile.SetWall(Directions.Direction.North, false);
            Assert.AreEqual(tile.GetWall(Directions.Direction.North), true);

        }


    }
}
