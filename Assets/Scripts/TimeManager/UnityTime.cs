using UnityEngine;
public class UnityTime : ITimeProvider
{
    public float Time => UnityEngine.Time.time;
    public float DeltaTime => UnityEngine.Time.deltaTime;

    public float TimeScaling
    {
        get => UnityEngine.Time.timeScale;
        set => UnityEngine.Time.timeScale = value;
    }

    private static ITimeProvider instance;

    public static ITimeProvider Instance { get => instance; set => instance = value; }

    static UnityTime()
    {
        instance = new UnityTime();
    }
}