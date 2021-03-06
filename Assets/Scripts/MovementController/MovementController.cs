

using System;
using System.Collections.Generic;
using GameManager;
using Logging;
using PubSubSystem;
using StateMachineCollection;
using UnityEngine;


public class MovementController : IMovementController, IDisposable
{
    private readonly PlayerCallbacks callbacks;
    private readonly ICombatManager combatManager;
    public ICombatManager CombatManager { get => combatManager; }
    private readonly IPlayerMover playerMover;
    private readonly Stack<Vector2Int> path;
    private Vector2Int position;
    public Vector2Int GridPosition { get => position; set => position = value; }
    LilLogger logger;
    static MovementController instance;
    public static MovementController Instance { get => instance; }
    Subscription<EventType> combatSubscriber;
    StateMachine machine;

    public Vector3 WorldPosition { get => playerMover.WorldPosition; }
    public MovementController(IMap map,
                            IPathFinder pathFinder,
                            GameManager.PlayerCallbacks callbacks,
                            ICombatManager combatManager,
                            IPlayerMover playerMover)
    {
        logger = new LilLogger("PlayerController");
        Debug.Assert(pathFinder != null);
        Debug.Assert(map != null);
        this.position = map.Start;
        this.callbacks = callbacks;
        this.combatManager = combatManager;
        this.playerMover = playerMover;
        path = pathFinder.FindPath(position, map.Goal, map);
        if (path.Count == 0)
        {
            logger.Log("Path generated 0 steps", LogLevel.Warning);
            callbacks.OnPlayerReachedGoal();
        }

        machine = new StateMachine(new DetermineStepState(this));

        combatSubscriber = SingletonProvider.MainEventPublisher.Subscribe(new[] { EventType.CombatResultsClosed }, (e) =>
          {
              if (e is CombatResultsClosedEvent ev)
              {
                  if (ev.Result.PlayerWon)
                  {
                      machine.RaiseEvent(new AwaitCombatState.CombatFinishedEvent());
                  }
                  else
                  {
                      callbacks.OnPlayerDied(); // tbh should probably be refactored to use pubsub
                  }
              }
          });
        if (instance != null) logger.Log("Replacing singleton instance");
        instance = this;
        SingletonProvider.MainPlayerController = instance; // Nasty. PlayerController should be reworked to be a proper singleton that always lives or not a singleton
    }
    public void Update()
    {
        if (!machine.IsTerminated()) machine.Update();
    }

    public void Dispose()
    {
        combatSubscriber.Dispose();
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
        callbacks.OnPlayerReachedGoal();
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
