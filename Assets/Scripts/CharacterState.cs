using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterState {

    public string stateName {get; protected set;}
    public Character cha {get; protected set; }
    public CharacterStateMachine sm {get; protected set; }

    protected float stateTimer;
    protected bool animeFinished;
    protected bool isBusy;


    public CharacterState(string stateName, Character cha) {
        this.stateName = stateName;
        this.cha = cha;
        this.sm = cha.sm;
        animeFinished = false;
    }
    public virtual void Enter () {
        stateTimer = 0;
        animeFinished = false;
        cha.anim.SetBool(stateName, true);
        cha.isBusy = isBusy; 
    }
    public virtual void Update() {
        stateTimer += Time.deltaTime;
    }
    public virtual void Exit()
    {
        cha.anim.SetBool(stateName, false);
        cha.isBusy = isBusy ? false : cha.isBusy;  
    }

    public virtual void AnimeFinish() {
        animeFinished = true;
     }
}
