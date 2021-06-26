// Need a syncronous timer
public class SimpleTimer
{
    private float timeout;
    private float startTime;
    public SimpleTimer(float timeout)
    {
        this.timeout = timeout;
        Reset();
    }

    public bool IsDone()
    {
        return startTime + timeout < SingletonProvider.MainTimeProvider.Time;
    }

    public void Reset()
    {
        startTime = SingletonProvider.MainTimeProvider.Time;
    }
}