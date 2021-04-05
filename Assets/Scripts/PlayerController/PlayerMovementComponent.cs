using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
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
    public Vector3 TargetPosition { get => targetPosition; }

    // instantly place the player at this position
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
        targetPosition = transform.position;
    }

    // The player will start moving towards this position
    public void SetTargetPosition(Vector3 pos)
    {
        if (!targetProvided)
        {
            transform.LookAt(pos);

        }
        originalPos = targetPosition;
        targetPosition = pos;
        startTime = UnityTime.Instance.Time;
        journeyLength = Vector3.Distance(originalPos, pos);
        targetProvided = true;
    }

    private void Update()
    {
        if (targetProvided)
        {
            transform.position = CalculateNextPosition();
            transform.rotation = CalculateNextRotation();
        }
    }

    private Quaternion CalculateNextRotation()
    {
        var direction = (targetPosition - transform.position).normalized;
        if (direction == Vector3.zero) return transform.rotation;
        var lookRotation = Quaternion.LookRotation(direction);
        return Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * UnityTime.Instance.DeltaTime);
    }

    private Vector3 CalculateNextPosition()
    {
        float distCovered = (UnityTime.Instance.Time - startTime) * movementSpeed;
        float fractionOfJourney = distCovered / journeyLength;
        return Vector3.Lerp(originalPos, targetPosition, fractionOfJourney);
    }

}