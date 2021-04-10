public interface ITurnProgress
{
    void ResetTurnProgress();
    bool IncrementTurnProgress(float speed);
}
public class TurnProgress : ITurnProgress
{
    public const float MaxTurnProgress = 100;
    private float turnProgress;

    public void ResetTurnProgress()
    {
        turnProgress = 0;
    }

    // Return true if it's their turn
    public bool IncrementTurnProgress(float speed)
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