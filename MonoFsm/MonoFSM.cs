using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoFSM<TState, TFsm> : MonoBehaviour
where TState : MonoStateBase<TState, TFsm>
where TFsm : MonoFSM<TState, TFsm> {
    //  Dictionary<Type, StateBase<T>> states = new Dictionary<Type, StateBase<T>>();
    Dictionary<string, TState> states = new Dictionary<string, TState>();
    protected TState currentState;
    public bool logStateChanges = false;
    //    public abstract void Init();

    public TState AddState(Type stateType) {
        try {
            TState state = (TState) Activator.CreateInstance(stateType);
            state.fsm = (TFsm) this;
            states[stateType.Name] = state;
            return state;
        } catch (InvalidCastException e) {
            throw new Exception("Type " + stateType + " should extend StateBase" + e);
        }
    }

    public TD AddState<TD>() where TD : TState {
        TState state = (TState) Activator.CreateInstance(typeof(TD));
        state.fsm = (TFsm)this;
        states[typeof(TD).Name] = state;
        return (TD) state;
    }

    public virtual void ChangeState<TD>() where TD : TState {
        ChangeState(typeof(TD).Name);
    }

    public void ChangeState(Type type) {
        ChangeState(type.Name);
    }
    public void ChangeState(string type) {
        if (logStateChanges) {
            Debug.Log(GetType() + ":: Change state to: " + type);
        }
        if (currentState != null) {
            currentState.Leave();
        }
        if (!states.ContainsKey(type)) throw new ArgumentException("FSM " + this.GetType().ToString()  + " doesnt contain state " + type);
        currentState = states[type];
        currentState.Enter();
    }

    public virtual void  Update() {
        if (currentState != null) {
            currentState.Update();
        }
    }

    public bool ActiveStateIs<stateType>() {
        return currentState == states[typeof(stateType).Name];
    }

    public TState GetState(string name) {
        return states[name];
    }




     public void Enable() {
        enabled = true;
        if (currentState != null) {
            currentState.Enter();
        }
    }

    // todo in case of changing state during disabled, enter/leave can be called twice
     public void Disable() {
        enabled = false;
        if (currentState != null) {
            currentState.Leave();
        }
    }

}