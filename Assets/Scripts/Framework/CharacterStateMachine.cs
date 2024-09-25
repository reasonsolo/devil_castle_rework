using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine 
{
    private int maxHistoricalQueue = 10;
    public Character cha { get; protected set; }
    public CharacterState currState;
    public CharacterState defaultState;
    public Queue<CharacterState> historicalStates { get; private set; }

    public CharacterStateMachine(Character cha) {
        this.cha = cha;
        historicalStates = new Queue<CharacterState>();
    }
    public void Initialize(CharacterState state)
    {   
        defaultState = state;
        currState = state;
        currState.Enter();
    }
    public void ChangeState(CharacterState newState)
    {
        if (cha.isBusy && currState.priority >= newState.priority)
        {
            Debug.Log("char is busy " + currState.stateName + " can't be interuptted by " + newState.stateName);
            return;
        }
        currState.Exit();
        currState = newState;
        currState.Enter();
    }
    public virtual void ChangeDefault()
    {
        Debug.Log("change to default " + defaultState);
        ChangeState(defaultState);
    }

    public void Update() {
        currState.Update();
    }
    ~CharacterStateMachine() {
        currState.Exit();
    }
}
