

using System;
using System.Collections.Generic;
using Logging;
using UnityEngine;

namespace PlayerController
{
    public class PlayerController
    {
        private readonly ITimeProvider timeProvider;
        private readonly Action onPathDone;
        private readonly Stack<Vector2Int> path;
        private Vector2Int position;
        public Vector2Int Position { get => position; }

        public float TimePerStep => timePerStep;

        private float previousStepTime;
        float timePerStep = 1;
        LilLogger logger;
        public PlayerController(IGridMap map, ITimeProvider timeProvider, IPathFinder pathFinder, Action onPathDone)
        {
            logger = new LilLogger("PlayerController");
            Debug.Assert(timeProvider != null);
            Debug.Assert(pathFinder != null);
            Debug.Assert(map != null);
            this.position = new Vector2Int(0, 0);
            while(map.GetTile(position).IsClosed()) {
                position += new Vector2Int(1, 1);
                if(position == map.Size) throw new Exception("Error horror");
            }

            var goal = map.Size - new Vector2Int(1, 1);
            while (map.GetTile(goal).IsClosed())
            {
                goal -= new Vector2Int(1, 1);
                if(goal == Vector2Int.zero) throw new Exception("Error horror");
            }
            this.timeProvider = timeProvider;
            this.onPathDone = onPathDone;
            path = pathFinder.FindPath(position, goal, map);
            previousStepTime = timeProvider.Time;
            if (path.Count == 0)
            {
                logger.Log("Path generated 0 steps", LogLevel.Warning);
                if (onPathDone != null) onPathDone();
            }

        }

        public void Update()
        {

            if (timeProvider.Time > previousStepTime + timePerStep && path.Count > 0)
            {
                previousStepTime = timeProvider.Time;
                this.position = path.Pop();
                if (path.Count == 0 && onPathDone != null)
                {
                    onPathDone();
                }
            }
        }

    }
}