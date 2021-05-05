using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logging;
namespace StateMachineCollection
{

    public interface IStateEvent
    {
    }

    public enum EventResult
    {
        EventHandled,
        EventNotHandled
    }

    public abstract class State
    {

        private bool machineTerminated = false;

        public bool MachineTerminated { get => machineTerminated; }

        public State() { }
        public virtual void OnEntry() { }
        public abstract State OnDuring();
        public virtual void OnExit() { }

        public virtual EventResult HandleEvent(IStateEvent happening)
        {
            return EventResult.EventNotHandled;
        }
        protected void TerminateMachine()
        {
            machineTerminated = true;
        }

    }

    public class StateMachine
    {
        State activeState;
        bool isFirstEntryExecuted = false;
        static readonly LilLogger logger = new LilLogger(typeof(StateMachine).ToString());
        public StateMachine(State initialState)
        {
            if (initialState == null) throw new System.Exception("Initial state is null");
            activeState = initialState;
        }

        public bool IsTerminated()
        {
            return activeState.MachineTerminated;
        }

        public void Update()
        {

            if (!activeState.MachineTerminated)
            {
                if (!isFirstEntryExecuted)
                {
                    activeState.OnEntry();
                    isFirstEntryExecuted = true;
                }
                State nextState = activeState.OnDuring();
                if (nextState != null)
                {
                    activeState.OnExit();
                    TransitToState(nextState);
                }
            }
        }

        private void TransitToState(State nextState)
        {
            activeState = nextState;
            if (!activeState.MachineTerminated)
            {
                activeState.OnEntry();
            }

        }

        public void RaiseEvent(IStateEvent ev)
        {
            if (activeState != null && !activeState.MachineTerminated)
            {
                var result = activeState.HandleEvent(ev);
                if (result == EventResult.EventNotHandled)
                {
                    logger.Log("Unhandled event in statemachine", LogLevel.Warning);
                }
            }
        }

        public static State NoTransition()
        {
            return null;
        }
    }

}