using StateMachineCollection;
using UnityEngine;

public class GoToTargetState : State
{
    public class PositionReachedEvent : IStateEvent { }
    private IPlayerController controller;
    private bool positionReached = false;

    public GoToTargetState(IPlayerController controller)
    {
        this.controller = controller;
        controller.RequestMoveTo(controller.Position);
    }

    public override void HandleEvent(IStateEvent happening)
    {
        if (happening is PositionReachedEvent) positionReached = true;
    }

    public override State OnDuring()
    {
        if (positionReached)
        {
            return new DetermineStepState(controller);
        }
        return StateMachine.NoTransition();
    }
}