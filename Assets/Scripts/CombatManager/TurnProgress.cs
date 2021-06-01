public interface ITurnProgress
{
    void ResetTurnProgress();
    bool IncrementTurnProgress(double speed);
}
public class TurnProgress : ITurnProgress
{
    public const double MaxTurnProgress = 100;
    private double turnProgress;

    public void RandomizeProgress()
    {
        turnProgress = SingletonProvider.MainRandomProvider.RandomFloat(0, (float)MaxTurnProgress);
    }

    public void ResetTurnProgress()
    {
        turnProgress = 0;
        RandomizeProgress();
    }

    // Return true if it's their turn
    public bool IncrementTurnProgress(double speed)
    {
        turnProgress += speed;
        if (turnProgress >= MaxTurnProgress)
        {
            turnProgress -= MaxTurnProgress;
            return true;
        }
        return false;
    }
}