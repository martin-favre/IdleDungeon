using StateMachineCollection;
using UnityEngine;

namespace GameManager
{

    public class PlayerReachedGoalEvent : IStateEvent
    {
    }

    public class PlayerDiedEvent : IStateEvent
    {

    }
    public class PlayGameState : State
    {
        private readonly IGameManager manager;
        private bool goalReached = false;
        private bool playerDied = false;

        public PlayGameState(IGameManager manager)
        {
            this.manager = manager;
        }

        public override void OnEntry()
        {
            manager.SpawnMap();
            manager.SpawnPlayer();
            manager.FadeIn();
        }
        public override EventResult HandleEvent(IStateEvent happening)
        {
            if (happening is PlayerReachedGoalEvent)
            {
                goalReached = true;
                Debug.Log("Hurrah");
                return EventResult.EventHandled;
            }
            if (happening is PlayerDiedEvent)
            {
                playerDied = true;
                Debug.Log("Boho");
                return EventResult.EventHandled;
            }
            return EventResult.EventNotHandled;
        }

        public override State OnDuring()
        {
            if (playerDied)
            {
                return new FadeOutState(manager, true);
            }
            if (goalReached)
            {
                return new FadeOutState(manager, false);
            }
            return StateMachine.NoTransition();
        }
    }

}