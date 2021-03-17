using UnityEngine;
public class UnityTime : ITimeProvider
{
    public float Time => timeOffset + UnityEngine.Time.unscaledTime * TimeScaling;
    public float DeltaTime => timeOffset + UnityEngine.Time.unscaledDeltaTime * TimeScaling;

    private float timeScaling;
    public float TimeScaling
    {
        get => timeScaling;
        set
        {
            if(value < 0) {
                Debug.LogWarning("Can't set timescaling to negative value");
                return;
            }
            var oldTime = Time;
            timeScaling = value;
            if(Time < oldTime) {
                timeOffset = oldTime;
            }
        }
    }

    private static ITimeProvider instance;

    private float timeOffset = 0;
    public static ITimeProvider Instance { get => instance; set => instance = value; }

    static UnityTime()
    {
        instance = new UnityTime();
    }
}