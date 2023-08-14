using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{   

    [Header("Movement info")]
    public float moveSpeed;
    public float idleTime;

    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake() {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Start(){
        base.Start();
    }

    protected override void Update() {
        base.Update();
        stateMachine.currentState.Update();
    }
}
