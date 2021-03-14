using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GridMapTest
    {
        [Test]
        public void TilesNullByDefault()
        {
            GridMap map = new GridMap(new Vector2Int(10, 10));
            Assert.IsNull(map.GetTile(new Vector2Int(5, 5)));
        }

        [Test]
        public void ShouldBeAbleToAddTileInsideSize()
        {
            GridMap map = new GridMap(new Vector2Int(10, 10));
            map.SetTile(new Vector2Int(5, 5), new Tile());
            Assert.IsNotNull(map.GetTile(new Vector2Int(5, 5)));
        }
        
        [Test]
        public void ShouldNotBeAbleToAddTileOutsideSize()
        {
            GridMap map = new GridMap(new Vector2Int(10, 10));
            bool gotException = false;
            try{
                map.SetTile(new Vector2Int(10, 10), new Tile());
            } catch(System.Exception) {
                gotException = true;
            }
            Assert.IsTrue(gotException);
        }
        [Test]
        public void ShouldNotBeAbleToGetTileOutsideSize()
        {
            GridMap map = new GridMap(new Vector2Int(10, 10));
            bool gotException = false;
            try{
                map.GetTile(new Vector2Int(10, 10));
            } catch(System.Exception) {
                gotException = true;
            }
            Assert.IsTrue(gotException);
        }

    }
}
