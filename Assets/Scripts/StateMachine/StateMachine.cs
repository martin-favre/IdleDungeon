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

        public bool MachineTerminated { get => machineTerminated; }

        public State() { }
        public virtual void OnEntry() { }
        public abstract State OnDuring();
        public virtual void OnExit() { }

        public virtual void HandleEvent(IStateEvent happening) { }
        protected void TerminateMachine()
        {
            machineTerminated = true;
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
                activeState.HandleEvent(ev);
            }
        }

        public static State NoTransition()
        {
            return null;
        }
    }

}