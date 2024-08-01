using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FSMPlayer 
{
    private PlayerFSMState CurrentState { get; set; }
    private Dictionary<Type, PlayerFSMState> _states = new Dictionary<Type, PlayerFSMState>();


    public void AddState(PlayerFSMState state)
    {
        _states.Add(state.GetType(), state);
    }

    public PlayerFSMState GetState()
    {
        return CurrentState;
    }

    public void SetState<T>() where T : PlayerFSMState
    {
        var type = typeof(T);

        if (CurrentState != null && CurrentState.GetType() == type)
            return;

        if (_states.TryGetValue(type, out var newState))
        {
            CurrentState?.Exit();

            CurrentState = newState;



            CurrentState.Enter();
        }
    }

    public void Update()
    {
        CurrentState.Update();
    }
}

public abstract class PlayerFSMState
{
    private protected FSMPlayer fsm;
    public PlayerFSMState(FSMPlayer _fsm)
    {
        fsm = _fsm;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
}

public class IdleState : PlayerFSMState
{
    

    public IdleState(FSMPlayer _fsm) : base(_fsm)
    {
    }

    public override void Enter()
    {
        Debug.Log("[IDLE STATE ENTER]");
    }

    public override void Exit()
    {
        Debug.Log("[IDLE STATE EXIT]");
    }

    public override void Update()
    {
        if(Input.GetAxisRaw("Horizontal") != 0|| Input.GetAxisRaw("Vertical") != 0)
        {
            fsm.SetState<WalkState>();
        }
    }
}
