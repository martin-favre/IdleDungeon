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
        controller.RequestMoveTo(controller.GridPosition);
    }

    public override EventResult HandleEvent(IStateEvent happening)
    {
        if (happening is PositionReachedEvent)
        {
            positionReached = true;
            return EventResult.EventHandled;
        }
        return EventResult.EventNotHandled;
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