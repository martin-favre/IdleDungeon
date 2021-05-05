using GameManager;
using StateMachineCollection;

public class FadeOutState : State
{
    private readonly IGameManager manager;
    private readonly bool playerDied;
    public const float fadeoutTime = 1.5f;
    private readonly float startTime;

    public FadeOutState(IGameManager manager, bool playerDied)
    {
        this.manager = manager;
        this.playerDied = playerDied;
        startTime = manager.TimeProvider.Time;
    }

    public override void OnEntry()
    {
        manager.FadeOut();
    }

    public override State OnDuring()
    {
        if (manager.TimeProvider.Time > startTime + fadeoutTime)
        {
            return new UpdatePointsState(manager, playerDied);
        }
        return StateMachine.NoTransition();
    }
}