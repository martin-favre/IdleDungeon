using StateMachineCollection;
using UnityEngine;

public class DetermineStepState : State
{
    public class TurningFinishedEvent : IStateEvent
    {

    }
    IPlayerController controller;
    bool turningFinished = false;

    public DetermineStepState(IPlayerController controller)
    {
        this.controller = controller;
    }

    public override void OnEntry()
    {
        if (controller.HasNextStep())
        {
            var nextStep = controller.GetNextStep();
            controller.RequestLookAt(nextStep);
            controller.Position = nextStep;
        }
        else
        {
            controller.NotifyPathFinished();
            TerminateMachine();
        }
    }

    public override void HandleEvent(IStateEvent happening)
    {
        if (happening is TurningFinishedEvent)
        {
            turningFinished = true;
        }
    }

    public override State OnDuring()
    {
        if (turningFinished)
        {
            bool combat = controller.CombatManager.PlayerEntersTile(controller.Position);
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