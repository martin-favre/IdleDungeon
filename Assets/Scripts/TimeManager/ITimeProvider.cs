public interface ITimeProvider
{
    float Time { get; }
    float DeltaTime { get; }

    float TimeScaling {get; set;}
}