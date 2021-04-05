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
            controller = new PlayerController(map, UnityTime.Instance, new DepthFirst(), OnDone, CombatManager.Instance);
            previousPosition = controller.Position;

            movementComponent = GetComponent<PlayerMovementComponent>();
            var pos = Helpers.ToVec3(previousPosition * Constants.tileSize, transform.position.y);
            movementComponent.SetPosition(pos);
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
                if (controller.Position != previousPosition)
                {
                    previousPosition = controller.Position;
                    if (!controller.IsDone())
                    {
                        targetPosition = Helpers.ToVec3(previousPosition * Constants.tileSize, Constants.tileSize.y / 2);
                    }
                    else
                    {
                        // last step, down a stair
                        // go down a bit, and slower
                        movementComponent.MovementSpeed = movementComponent.MovementSpeed / 2f;
                        targetPosition = Helpers.ToVec3(previousPosition * Constants.tileSize, (Constants.tileSize.y / 2 - Constants.tileSize.y * 0.2f));
                    }
                    movementComponent.SetTargetPosition(targetPosition);
                }
            }
        }
    }

}