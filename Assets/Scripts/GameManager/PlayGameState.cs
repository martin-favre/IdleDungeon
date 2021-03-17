using StateMachineCollection;
using UnityEngine;

namespace GameManager
{

    public class PlayerReachedGoalEvent : IStateEvent
    {

    }
    public class PlayGameState : State
    {
        private readonly IGameManager manager;
        private bool goalReached = false;

        public PlayGameState(IGameManager manager)
        {
            this.manager = manager;
        }

        public override void OnEntry()
        {
            manager.SpawnMaze();
            manager.SpawnPlayer();
        }
        public override void HandleEvent(IStateEvent happening)
        {
            if (happening is PlayerReachedGoalEvent)
            {
                var ev = happening as PlayerReachedGoalEvent;
                goalReached = true;
                Debug.Log("Hurrah");
            }
        }

        public override State OnDuring()
        {
            if(goalReached) {
                return new UpdatePointsState(manager);
            }
            return StateMachine.NoTransition();
        }
    }

}