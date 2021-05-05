using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{

    public class PlayerControllerComponent : MonoBehaviour
    {
        private PlayerController controller;
        private Action onGoalReached;

        private Vector2Int previousPosition;

        private Vector3 targetPosition;
        float startTime;
        float journeyLength;
        private Vector3 originalPos;
        PlayerMovementComponent movementComponent;

        public void Setup(IMap map, Action onGoalReached, Action OnPlayerDied)
        {
            this.onGoalReached = onGoalReached;
            movementComponent = GetComponent<PlayerMovementComponent>();
            controller = new PlayerController(map,
                                              new DepthFirst(),
                                              OnDone,
                                              OnPlayerDied,
                                              CombatManager.Instance,
                                              movementComponent);
            previousPosition = controller.Position;

            movementComponent.SetPosition(previousPosition);
        }

        private void OnDestroy()
        {
            controller.Dispose();
        }

        private void OnDone()
        {
            if (onGoalReached != null) onGoalReached();
        }

        private void Update()
        {
            if (controller != null)
            {
                controller.Update();
            }
        }
    }

}