using StateMachineCollection;
using UnityEngine;

public class AwaitCombatState : State
{
    public class CombatFinishedEvent :IStateEvent {}
    private IPlayerController controller;
    bool combatFinished = false;

    public AwaitCombatState(IPlayerController controller)
    {
        this.controller = controller;
    }

    public override EventResult HandleEvent(IStateEvent happening)
    {
        if(happening is CombatFinishedEvent){
             combatFinished = true;
             return EventResult.EventHandled;
        }
        return EventResult.EventNotHandled;
    }

    public override State OnDuring()
    {
        if(combatFinished) {
            return new GoToTargetState(controller);
        }
        return StateMachine.NoTransition();
    }
}