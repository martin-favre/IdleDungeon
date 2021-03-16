using UnityEngine;
public class UnityTime : ITimeProvider
{
    public float Time => UnityEngine.Time.unscaledTime;
    public float DeltaTime => UnityEngine.Time.unscaledDeltaTime;

    private static ITimeProvider instance;

    public static ITimeProvider Instance { get => instance; set => instance = value; }

    static UnityTime() {
        instance = new UnityTime();
    }
}