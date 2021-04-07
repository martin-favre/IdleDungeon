

using System;
using System.Collections.Generic;
using Logging;
using StateMachineCollection;
using UnityEngine;

namespace PlayerController
{
    public class PlayerController : IPlayerController, IDisposable
    {
        private readonly Action onPathDone;
        private readonly ICombatManager combatManager;
        private readonly IPlayerMover playerMover;
        public ICombatManager CombatManager { get => combatManager; }
        private readonly Stack<Vector2Int> path;
        private Vector2Int position;
        public Vector2Int Position
        {
            get => position;
            set
            {
                movementDir = value - position;
                position = value;
            }
        }
        private Vector2Int movementDir;
        public Vector2Int MovementDir { get => movementDir; }
        LilLogger logger;
        static PlayerController instance;
        public static PlayerController Instance { get => instance; }
        SimpleObserver<CombatManagerUpdateEvent> combatObserver;
        StateMachine machine;

        public Vector3 WorldPosition { get => playerMover.WorldPosition; }
        public PlayerController(IMap map,
                                IPathFinder pathFinder,
                                Action onPathDone,
                                ICombatManager combatManager,
                                IPlayerMover playerMover)
        {
            logger = new LilLogger("PlayerController");
            Debug.Assert(pathFinder != null);
            Debug.Assert(map != null);
            this.position = map.Start;
            this.onPathDone = onPathDone;
            this.combatManager = combatManager;
            this.playerMover = playerMover;
            path = pathFinder.FindPath(position, map.Goal, map);
            if (path.Count == 0)
            {
                logger.Log("Path generated 0 steps", LogLevel.Warning);
                if (onPathDone != null) onPathDone();
            }

            machine = new StateMachine(new DetermineStepState(this));

            combatObserver = new SimpleObserver<CombatManagerUpdateEvent>(combatManager, (e) =>
            {
                if (e.Type == CombatManagerUpdateEvent.UpdateType.EnteredCombat)
                {
                    Debug.Log("Player enters combat");
                }
                else
                {
                    machine.RaiseEvent(new AwaitCombatState.CombatFinishedEvent());
                }
            });
            if (instance != null) logger.Log("Replacing singleton instance");
            instance = this;
        }
        public void Update()
        {
            if (!machine.IsTerminated()) machine.Update();
        }

        public bool IsDone()
        {
            return path.Count == 0;
        }

        public void Dispose()
        {
            combatObserver.Dispose();
        }

        public bool HasNextStep()
        {
            return path.Count != 0;
        }

        public Vector2Int GetNextStep()
        {
            return path.Pop();
        }

        public void NotifyPathFinished()
        {
            onPathDone();
        }

        public void RequestLookAt(Vector2Int position)
        {
            playerMover.RotateTowards(position, () => machine.RaiseEvent(new DetermineStepState.TurningFinishedEvent()));
        }

        public void RequestMoveTo(Vector2Int position)
        {
            playerMover.MoveTowards(position, () => machine.RaiseEvent(new GoToTargetState.PositionReachedEvent()));
        }
    }
}