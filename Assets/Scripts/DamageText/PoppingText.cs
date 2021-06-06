using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PoppingText : MonoBehaviour
{

    [SerializeField]
    float lifeDuration;

    [SerializeField]
    float initialForce;

    [SerializeField]
    float xDirectionAmount;

    float startTime;

    Rigidbody2D body;

    ITimeProvider timeProvider;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        var xDir = SingletonProvider.MainRandomProvider.RandomFloat(-xDirectionAmount, xDirectionAmount);
        body.AddForce(new Vector2(initialForce * xDir, initialForce), ForceMode2D.Impulse);
        timeProvider = SingletonProvider.MainTimeProvider;
        startTime = timeProvider.Time;
    }

    void Update()
    {
        if (startTime + lifeDuration < timeProvider.Time)
        {
            GameObject.Destroy(gameObject);
        }
    }
}