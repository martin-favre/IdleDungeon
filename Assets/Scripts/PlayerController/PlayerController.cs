

using System;
using System.Collections.Generic;
using Logging;
using UnityEngine;

namespace PlayerController
{
    public class PlayerController : IDisposable
    {
        private readonly ITimeProvider timeProvider;
        private readonly Action onPathDone;
        private readonly ICombatManager combatManager;
        private readonly Stack<Vector2Int> path;
        private Vector2Int position;
        public Vector2Int Position { get => position; }

        public float TimePerStep => timePerStep;
        public const float timePerStep = 1;
        private float previousStepTime;
        LilLogger logger;

        SimpleObserver<CombatManagerUpdateEvent> combatObserver;
        public PlayerController(IMap map,
                                ITimeProvider timeProvider,
                                IPathFinder pathFinder,
                                Action onPathDone,
                                ICombatManager combatManager)
        {
            logger = new LilLogger("PlayerController");
            Debug.Assert(timeProvider != null);
            Debug.Assert(pathFinder != null);
            Debug.Assert(map != null);
            this.position = map.Start;
            this.timeProvider = timeProvider;
            this.onPathDone = onPathDone;
            this.combatManager = combatManager;
            combatObserver = new SimpleObserver<CombatManagerUpdateEvent>(combatManager, (e) =>
            {
                if (e.Type == CombatManagerUpdateEvent.UpdateType.EnteredCombat)
                {
                    Debug.Log("Player enters combat");
                }
                else
                {
                    Debug.Log("Player left combat");
                }
            });
            path = pathFinder.FindPath(position, map.Goal, map);
            previousStepTime = 0; // so it will trigger right away 
            if (path.Count == 0)
            {
                logger.Log("Path generated 0 steps", LogLevel.Warning);
                if (onPathDone != null) onPathDone();
            }
        }
        public void Update()
        {
            if(combatManager.InCombat()) return;
            if (timeProvider.Time > previousStepTime + timePerStep && path.Count > 0)
            {
                previousStepTime = timeProvider.Time;
                this.position = path.Pop();
                if (path.Count == 0 && onPathDone != null)
                {
                    onPathDone();
                }
                else
                {
                    combatManager.PlayerEntersTile(this.position);
                }
            }
        }

        public bool IsDone()
        {
            return path.Count == 0;
        }

        public void Dispose()
        {
            combatObserver.Dispose();
        }
    }
}