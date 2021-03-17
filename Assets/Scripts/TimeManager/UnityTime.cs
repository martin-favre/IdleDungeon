using UnityEngine;
public class UnityTime : ITimeProvider
{
    public float Time => UnityEngine.Time.unscaledTime*TimeScaling;
    public float DeltaTime => UnityEngine.Time.unscaledDeltaTime*TimeScaling;

    private float timeScaling;
    public float TimeScaling  { get => timeScaling; set => timeScaling = value; }

    private static ITimeProvider instance;

    public static ITimeProvider Instance { get => instance; set => instance = value; }

    static UnityTime() {
        instance = new UnityTime();
    }
}