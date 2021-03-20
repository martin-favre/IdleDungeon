using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Logging;
namespace Tests
{
    public class RecursiveBacktrackerTest
    {
        static LilLogger logger = new LilLogger("RecursiveBacktrackerTest");

        [Test]
        public void GenerationShouldWork()
        {
            Vector2Int size = new Vector2Int(20, 20);
            IMap map = (new RecursiveBacktracker()).GenerateMap(size, 10);
            string str = GridMapHelper.ToString(map);
            // At this point I can't really evaluate the map so it's a visual check :D
            logger.Log('\n' + str);
        }

        [Test]
        public void OuterWallsShouldAlwaysBeClosed()
        {
            Vector2Int size = new Vector2Int(50, 50);
            IMap map = (new RecursiveBacktracker()).GenerateMap(size, 10);
            Helpers.DoForAll(size, (pos) =>
            {
                if (pos.x == 0) Assert.AreEqual(false, map.GetTile(pos).GetWall(Directions.Direction.West));
                if (pos.y == 0) Assert.AreEqual(false, map.GetTile(pos).GetWall(Directions.Direction.South));
                if (pos.x == size.x - 1) Assert.AreEqual(false, map.GetTile(pos).GetWall(Directions.Direction.East));
                if (pos.y == size.y - 1) Assert.AreEqual(false, map.GetTile(pos).GetWall(Directions.Direction.North));
            });
        }
        [Test]
        public void GeneratingTwiceShouldYieldSameResult()
        {
            Vector2Int size = new Vector2Int(20, 20);
            var mapGenerator = new RecursiveBacktracker();
            var map = mapGenerator.GenerateMap(size, 10);
            string str = GridMapHelper.ToString(map);
            var map2 = mapGenerator.GenerateMap(size, 10);
            string str2 = GridMapHelper.ToString(map2);
            Assert.AreEqual(str, str2);
        }

    }
}
