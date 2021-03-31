using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;
using PlayerController;

namespace Tests
{
    public class PlayerControllerTest
    {

        Mock<ITimeProvider> timeMock;
        float currentTime;
        Mock<IMap> mapMock;
        Vector2Int mapSize;
        Tile mockTile;

        Mock<IPathFinder> pathfinderMock;
        Stack<Vector2Int> path;

        [SetUp]
        public void Setup()
        {
            timeMock = new Mock<ITimeProvider>();
            currentTime = 0;
            timeMock.Setup(foo => foo.Time).Returns(() => currentTime);
            mapMock = new Mock<IMap>();
            mapSize = new Vector2Int(20, 20);
            mapMock.Setup(foo => foo.Size).Returns(mapSize);
            mockTile = new Tile();
            mockTile.SetAllWalls(true);
            mapMock.Setup(foo => foo.GetTile(It.IsAny<Vector2Int>())).Returns(mockTile);

            pathfinderMock = new Mock<IPathFinder>();
            path = new Stack<Vector2Int>();
            pathfinderMock.Setup(foo => foo.FindPath(It.IsAny<Vector2Int>(),
                                                    It.IsAny<Vector2Int>(),
                                                    It.IsAny<IMap>())).Returns(path);

        }

        [Test]
        public void PlayerShouldTakeStepEveryTimeUnit()
        {
            var controller = new PlayerController.PlayerController(mapMock.Object, timeMock.Object, pathfinderMock.Object, null);
            var playerPos = controller.Position;
            path.Push(controller.Position + new Vector2Int(1, 0));
            currentTime = controller.TimePerStep + 0.0001f; // just a bit more
            controller.Update();
            Assert.AreEqual(1, Mathf.Abs((playerPos.x + playerPos.y) -
            (controller.Position.x + controller.Position.y))); // One step
        }

        [Test]
        public void PlayerWillWalkUntilPathDone()
        {
            var controller = new PlayerController.PlayerController(mapMock.Object, timeMock.Object, pathfinderMock.Object, null);

            for (int i = 0; i < 10; i++)
            {
                path.Push(new Vector2Int(9 - i, 9 - i)); // Put path in reverse (goal first, start last)
            }
            for (int i = 0; i < 10; i++)
            {
                // increment time by time per step, plus a little bit
                // +1 so first update will be 1s and so on.
                currentTime = (controller.TimePerStep + 0.0001f) * (i + 1);
                controller.Update();
                Assert.AreEqual(new Vector2Int(i, i), controller.Position);
            }
        }

        [Test]
        public void PlayerShouldNotifyOnPathDone()
        {
            bool flagDone = false;
            var controller = new PlayerController.PlayerController(mapMock.Object, timeMock.Object, pathfinderMock.Object, () => flagDone = true);
            path.Push(new Vector2Int(1, 0));
            currentTime = controller.TimePerStep + 0.0001f; // just a bit more
            controller.Update();
            Assert.AreEqual(true, flagDone);
        }

        [Test]
        public void PlayerShouldNotifyOnEmptyPath()
        {
            bool flagDone = false;
            var controller = new PlayerController.PlayerController(mapMock.Object, timeMock.Object, pathfinderMock.Object, () => flagDone = true);
            currentTime = controller.TimePerStep + 0.0001f; // just a bit more
            controller.Update();
            Assert.AreEqual(true, flagDone);
        }

        [Test]
        public void IsDoneShouldReturnTrueOnEmptyPath()
        {
            var controller = new PlayerController.PlayerController(mapMock.Object, timeMock.Object, pathfinderMock.Object, null);
            currentTime = controller.TimePerStep + 0.0001f; // just a bit more

            controller.Update();
            Assert.AreEqual(true, controller.IsDone());
        }

        [Test]
        public void IsDoneShouldReturnFalseByDefault()
        {
            path.Push(Vector2Int.zero);
            var controller = new PlayerController.PlayerController(mapMock.Object, timeMock.Object, pathfinderMock.Object, null);
            Assert.AreEqual(false, controller.IsDone());
        }



    }
}
