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

    public override void HandleEvent(IStateEvent happening)
    {
        if(happening is CombatFinishedEvent) combatFinished = true;
    }

    public override State OnDuring()
    {
        if(combatFinished) {
            return new GoToTargetState(controller);
        }
        return StateMachine.NoTransition();
    }
}