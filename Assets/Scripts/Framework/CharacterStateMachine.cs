using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine 
{
    //private int maxHistoricalQueue = 10;
    public Character cha { get; protected set; }
    public CharacterState currState;
    public CharacterState defaultState;
    //public Queue<CharacterState> historicalStates { get; private set; }

    public CharacterStateMachine(Character cha) {
        this.cha = cha;
        //historicalStates = new Queue<CharacterState>();
    }
    public void Initialize(CharacterState state)
    {
        Debug.Log("intialize state " + state);
        defaultState = state;
        currState = state;
        currState.Enter();
    }
    public void ChangeState(CharacterState newState)
    {
        //if (cha.isBusy && currState.priority >= newState.priority)
        //{
        //    Debug.Log("char is busy " + currState.stateName + " can't be interuptted by " + newState.stateName);
        //    return;
        //}
        //Debug.Log("change from " + currState +  " to " + newState);
        currState.Exit();
        currState = newState;
        currState.Enter();
    }
    public virtual void ChangeDefault()
    {
        ChangeState(defaultState);
    }

    public void Update() {
        currState.Update();
    }
    ~CharacterStateMachine() {
        currState.Exit();
    }
}
