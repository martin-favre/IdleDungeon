using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineCollection
{

    public interface IStateEvent
    {
    }

    public abstract class State
    {

        private bool machineTerminated = false;

        public State() { }
        public virtual void OnEntry() { }
        public abstract State OnDuring();
        public virtual void OnExit() { }

        public virtual void HandleEvent(IStateEvent happening) { }
        protected void TerminateMachine()
        {
            machineTerminated = true;
        }
        public bool IsMachineTerminated()
        {
            return machineTerminated;
        }

    }

    public class StateMachine
    {
        State activeState;
        bool isFirstEntryExecuted = false;
        public StateMachine(State initialState)
        {
            if (initialState == null) throw new System.Exception("Initial state is null");
            activeState = initialState;
        }

        public bool IsTerminated()
        {
            return activeState.IsMachineTerminated();
        }

        public void Update()
        {

            if (!activeState.IsMachineTerminated())
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
            if (!activeState.IsMachineTerminated())
            {
                activeState.OnEntry();
            }

        }

        public void RaiseEvent(IStateEvent ev)
        {
            if (activeState != null && !activeState.IsMachineTerminated())
            {
                activeState.HandleEvent(ev);
            }
        }

        public static State NoTransition()
        {
            return null;
        }
    }

}