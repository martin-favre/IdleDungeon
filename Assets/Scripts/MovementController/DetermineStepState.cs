using StateMachineCollection;
using UnityEngine;

public class DetermineStepState : State
{
    public class TurningFinishedEvent : IStateEvent
    {

    }
    IMovementController controller;
    bool turningFinished = false;

    public DetermineStepState(IMovementController controller)
    {
        this.controller = controller;
    }

    public override void OnEntry()
    {
        if (controller.HasNextStep())
        {
            var nextStep = controller.GetNextStep();
            controller.RequestLookAt(nextStep);
            controller.GridPosition = nextStep;
        }
        else
        {
            controller.NotifyPathFinished();
            TerminateMachine();
        }
    }

    public override EventResult HandleEvent(IStateEvent happening)
    {
        if (happening is TurningFinishedEvent)
        {
            turningFinished = true;
            return EventResult.EventHandled;
        }
            return EventResult.EventNotHandled;
    }

    public override State OnDuring()
    {
        if (turningFinished)
        {
            bool combat = controller.CombatManager.PlayerEntersTile(controller.GridPosition);
            if (combat)
            {
                return new AwaitCombatState(controller);
            }
            else
            {
                return new GoToTargetState(controller);
            }
        }
        return StateMachine.NoTransition();
    }
}