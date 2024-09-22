using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine 
{
    private int maxHistoricalQueue = 10;
    public Character cha { get; protected set; }
    public CharacterState currState;
    public Queue<CharacterState> historicalStates { get; private set; }

    public CharacterStateMachine(Character cha) {
        this.cha = cha;
        historicalStates = new Queue<CharacterState>();
    }
    public void Initialize(CharacterState state)
    {   
        currState = state;
        currState.Enter();
    }
    public void ChangeState(CharacterState newState)
    {
        if (historicalStates.Count == maxHistoricalQueue)
        {
            historicalStates.Dequeue();
        }
        currState.Exit();
        historicalStates.Enqueue(currState);
        currState = newState;
        currState.Enter();
    }

    public void Update() {
        currState.Update();
    }
    ~CharacterStateMachine() {
        currState.Exit();
    }
}
