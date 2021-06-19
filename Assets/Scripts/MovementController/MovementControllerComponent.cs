using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementControllerComponent : MonoBehaviour
{
    private MovementController controller;

    private Vector2Int previousPosition;

    private Vector3 targetPosition;
    float startTime;
    float journeyLength;
    private Vector3 originalPos;
    PlayerMovementComponent movementComponent;

    public void Setup(IMap map, GameManager.PlayerCallbacks callbacks)
    {
        movementComponent = GetComponent<PlayerMovementComponent>();
        controller = new MovementController(map,
                                          new DepthFirst(),
                                          callbacks,
                                          SingletonProvider.MainCombatManager,
                                          movementComponent);
        previousPosition = controller.GridPosition;

        movementComponent.SetPosition(previousPosition);
    }

    private void OnDestroy()
    {
        controller.Dispose();
    }

    private void Update()
    {
        if (controller != null)
        {
            controller.Update();
        }
    }
}

