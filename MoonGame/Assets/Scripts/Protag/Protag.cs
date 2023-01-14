using System;
using System.Collections;
using System.Collections.Generic;
using MushiLWFSM;
using UnityEngine;

public class Protag : MonoBehaviour
{
    public enum MoveState
    {
        Idle,
    }

    [ColorHeader("Dependencies")] 
    [SerializeField] private InputProvider inputProvider;

    [ColorHeader("Debug Fields")]
    [ReadOnly, SerializeField] private MoveState currentState;
    
    // private fields
    private QStateMachine stateMachine;
    
    private void OnEnable()
    {
        stateMachine = new QStateMachine((int)MoveState.Idle);
    }

    private void Update()
    {
        stateMachine.UpdateStateMachine();
        currentState = (MoveState)stateMachine.CurrentState;
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdateStateMachine();
    }
    
    // State implementations here SINGLE FILE MONOLITH LETS FUCKING GOOOOOO
    
    #region Idle State

    public void IdleUpdate()
    {
        
    }

    public void IdleFixedUpdate()
    {

    }

    public void IdleEnterState()
    {

    }

    public void IdleExitState()
    {

    }

    public int IdleSwitchState()
    {
        return -1;
    }
    
    #endregion
}
