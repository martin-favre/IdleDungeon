using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControllerComponent : MonoBehaviour
{
    private PlayerController controller;

    private Vector2Int previousPosition;

    private Vector3 targetPosition;
    float startTime;
    float journeyLength;
    private Vector3 originalPos;
    PlayerMovementComponent movementComponent;

    public void Setup(IMap map, GameManager.PlayerCallbacks callbacks)
    {
        movementComponent = GetComponent<PlayerMovementComponent>();
        controller = new PlayerController(map,
                                          new DepthFirst(),
                                          callbacks,
                                          CombatManager.Instance,
                                          movementComponent);
        previousPosition = controller.Position;

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

