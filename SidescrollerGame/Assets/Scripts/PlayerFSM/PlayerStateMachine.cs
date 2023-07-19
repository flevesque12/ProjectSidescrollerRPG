using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This is the script that is the transition of each state and the blue print 
*/
public class PlayerStateMachine{
    //put it read only
    public PlayerState currentState{get; private set;}
    
    public void Initialize(PlayerState _startState){
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState){
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
