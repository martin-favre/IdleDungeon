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
            IGridMap map = (new RecursiveBacktracker()).GenerateMap(size, 10);
            string str = GridMapHelper.ToString(map);
            // At this point I can't really evaluate the map so it's a visual check :D
            logger.Log('\n' + str); 
        }

    }
}
