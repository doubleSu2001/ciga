using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateBase
{
    public string name;
    public float StateTick = 0;

    public virtual void Enter(MonoBehaviour Context)
    {
        StateTick = 0;
    }
    public virtual void Back(MonoBehaviour Context)
    {

    }
    public virtual StateBase Tick(float Delta, MonoBehaviour Context)
    {
        StateTick += Delta;
        return this;
    }
}

[System.Serializable]
public class StateMachine
{
    [SerializeField]
    public StateBase CurState;
    [HideInInspector]
    public MonoBehaviour Context;
    public StateMachine(StateBase State, MonoBehaviour context)
    {
        CurState = State;
        Context = context;
        State.Enter(Context);
    }
    public void Tick(float Delta)
    {
        StateBase NewState = CurState.Tick(Delta, Context);
        if(NewState != CurState)
        {
            CurState.Back(Context);
            NewState.Enter(Context);
            CurState = NewState;
        }
    }
}
