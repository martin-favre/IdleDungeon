public interface ITurnProgress
{
    float TurnTime { get; }
    bool IsDone();
    void Reset();
    // return float 0-1
    // 0, it's a long time left
    // 1, it's their turn asap
    float GetRelativeProgress();
}
public class TurnProgress : ITurnProgress
{
    public TurnProgress(float turnTime)
    {
        this.turnTime = turnTime;
        Reset();
    }
    private float startTime;
    private readonly float turnTime;

    public float TurnTime => turnTime;

    public bool IsDone()
    {
        return startTime + turnTime < SingletonProvider.MainTimeProvider.Time;
    }
    public float GetRelativeProgress()
    {
        if (IsDone()) return 1;
        var howMuchTimeHasPassed = SingletonProvider.MainTimeProvider.Time - startTime;
        var progress = howMuchTimeHasPassed / turnTime;
        return progress;
    }

    public void Reset()
    {
        this.startTime = SingletonProvider.MainTimeProvider.Time;
    }
}