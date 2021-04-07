using System;
using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour, IPlayerMover
{
    [SerializeField]
    private Vector3 positionToLookAt;
    [SerializeField]
    private Vector3 targetPosition;


    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float movementSpeed = Constants.tileSize.x;

    private float startTime;
    private Vector3 originalPos;
    private float journeyLength;
    bool targetProvided = false;

    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

    public Vector3 WorldPosition { get => transform.position; }

    private Action onRotateDone;
    private Action onMoveDone;

    private bool rotating = false;
    protected bool moving = false;

    Vector3 GridPosToRealPos(Vector2Int pos)
    {
        return Helpers.ToVec3(pos * Constants.tileSize, transform.position.y);
    }

    // instantly place the player at this position
    public void SetPosition(Vector2Int pos)
    {
        transform.position = GridPosToRealPos(pos);
        positionToLookAt = transform.position;
    }

    // The player will start moving towards this position
    private void Update()
    {
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            bool done;
            (transform.position, done) = CalculateNextPosition();
            if (done)
            {
                moving = false;
                onMoveDone();
            }
        }

        if (rotating)
        {
            bool done;
            (transform.rotation, done) = CalculateNextRotation();
            if (done)
            {
                // transform.LookAt(positionToLookAt);
                rotating = false;
                onRotateDone();
            }
        }

    }

    private (Quaternion, bool) CalculateNextRotation()
    {
        var direction = (positionToLookAt - transform.position).normalized;
        if (direction == Vector3.zero) return (transform.rotation, true); // Basically if we're on the position we want to look at, then things go bananas 
        var lookRotation = Quaternion.LookRotation(direction);
        var newRot = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * UnityTime.Instance.DeltaTime);
        var ridiculousMargin = 0.00000001f; // if it's too big, the jump to the correct rotation looks odd, if it's too small the rotation takes forever
        if (Helpers.Approximately(newRot, transform.rotation, ridiculousMargin)) return (lookRotation, true);
        return (newRot, false);
    }

    private (Vector3, bool) CalculateNextPosition()
    {
        float distCovered = (UnityTime.Instance.Time - startTime) * movementSpeed;
        float fractionOfJourney = distCovered / journeyLength;
        var ridiculousMargin = 0.00000001f; // if it's too big, there'll be a small jerk between steps, if it's too small the movement takes forever
        if ((transform.position - targetPosition).magnitude < ridiculousMargin)
        {
            return (transform.position, true);
        }
        return (Vector3.Lerp(originalPos, targetPosition, fractionOfJourney), false);
    }

    public void RotateTowards(Vector2Int posToLookAt, Action notifyDone)
    {
        positionToLookAt = GridPosToRealPos(posToLookAt);
        var direction = (positionToLookAt - transform.position).normalized;
        if (direction == Vector3.zero)
        {
            notifyDone();
            return;
        }
        var lookRotation = Quaternion.LookRotation(direction);
        if (lookRotation == transform.rotation)
        {
            notifyDone();
            return;
        }
        onRotateDone = notifyDone;
        rotating = true;
    }

    public void MoveTowards(Vector2Int posToGoTo, Action notifyDone)
    {
        originalPos = transform.position;
        targetPosition = GridPosToRealPos(posToGoTo);
        journeyLength = Vector3.Distance(originalPos, targetPosition);
        onMoveDone = notifyDone;
        startTime = UnityTime.Instance.Time;
        moving = true;
    }
}