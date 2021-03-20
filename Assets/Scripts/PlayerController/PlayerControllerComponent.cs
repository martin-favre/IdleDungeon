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

        public void Setup(IMap map, Action onGoalReached)
        {
            this.onGoalReached = onGoalReached;
            controller = new PlayerController(map, UnityTime.Instance, new DepthFirst(), OnDone);
            previousPosition = controller.Position;

            movementComponent = GetComponent<PlayerMovementComponent>();
            var pos = Helpers.ToVec3(previousPosition * Constants.tileSize, transform.position.y);
            movementComponent.SetPosition(pos);

        }

        void Awake()
        {
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
                if (controller.Position != previousPosition)
                {
                    previousPosition = controller.Position;
                    targetPosition = Helpers.ToVec3(previousPosition * Constants.tileSize, transform.position.y);
                    movementComponent.SetTargetPosition(targetPosition);
                }
            }
        }
    }

}