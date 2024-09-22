using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine 
{   
    public Character cha { get; protected set; }
    public CharacterState currState;

    public CharacterStateMachine(Character cha) {
        this.cha = cha;
    }
    public void Initialize(CharacterState state)
    {   
        currState = state;
        currState.Enter();
    }
    public void ChangeState(CharacterState newState)
    {
        currState.Exit();
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
