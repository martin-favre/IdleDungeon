using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
    private Vector3 targetPosition;

    [SerializeField]
    private float rotationSpeed;

    private float movementSpeed = Constants.tileSize.x; // we want to complete one tilesize per step

    private float startTime;
    private Vector3 originalPos;
    private float journeyLength;
    bool targetProvided = false;

    Vector3 targetPos;
    Quaternion targetRot;
    public void SetTargetPosition(Vector3 pos)
    {
        targetPosition = pos;
        startTime = UnityTime.Instance.Time;
        originalPos = transform.position;
        journeyLength = Vector3.Distance(transform.position, pos);
        targetProvided = true;
    }

    private void Update()
    {
        if (targetProvided)
        {
            UpdatePosition();
            UpdateRotation();
        }
    }

    private void FixedUpdate()
    {
        if (targetProvided)
        {
            transform.position = targetPos;
            transform.rotation = targetRot;
        }
    }

    private void UpdateRotation()
    {
        var direction = (targetPosition - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction);
        targetRot = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * UnityTime.Instance.DeltaTime);
    }

    private void UpdatePosition()
    {
        float distCovered = (UnityTime.Instance.Time - startTime) * movementSpeed;
        float fractionOfJourney = distCovered / journeyLength;
        targetPos = Vector3.Lerp(originalPos, targetPosition, fractionOfJourney);
    }

}