using System;
using System.Collections;
using System.Collections.Generic;
using MushiLWFSM;
using UnityEngine;

public class Protag : MonoBehaviour
{
    public enum ProtagState
    {
        Idle,
        Walk,
        Airborne,
        Jump,
        Jetpack,
        Stasis
    }

    [ColorHeader("Dependencies")] 
    [SerializeField] private InputProvider inputProvider;
    [SerializeField] private FPCameraController protagFPCam;

    [ColorHeader("Movement Dependencies")] 
    [SerializeField] private ProtagPhysicsState physicsState;
    [SerializeField] private ProtagMover protagMover;
    [SerializeField] private ProtagRotationResolver rotationResolver;

    [ColorHeader("Debug Fields")]
    [ReadOnly, SerializeField] private ProtagState currentState;
    
    // private fields
    private QStateMachine stateMachine;
    
    private void OnEnable()
    {
        stateMachine = new QStateMachine((int)ProtagState.Idle);
        AddStates();
    }

    private void Update()
    {
        stateMachine.UpdateStateMachine();
        currentState = (ProtagState)stateMachine.CurrentState;
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdateStateMachine();
    }
    
    #region Add States
    private void AddStates()
    {
        stateMachine.AddNewState(
            (int)ProtagState.Idle,
            IdleUpdate,
            IdleFixedUpdate,
            IdleEnterState,
            IdleExitState,
            IdleSwitchState);

        stateMachine.AddNewState(
            (int)ProtagState.Walk,
            WalkUpdate,
            WalkFixedUpdate,
            WalkEnterState,
            WalkExitState,
            WalkSwitchState);

        stateMachine.AddNewState(
            (int)ProtagState.Airborne,
            AirborneUpdate,
            AirborneFixedUpdate,
            AirborneEnterState,
            AirborneExitState,
            AirborneSwitchState);

        stateMachine.AddNewState(
            (int)ProtagState.Jetpack,
            JetpackUpdate,
            JetpackFixedUpdate,
            JetpackEnterState,
            JetpackExitState,
            JetpackSwitchState);

        stateMachine.AddNewState(
            (int)ProtagState.Stasis,
            StasisUpdate,
            StasisFixedUpdate,
            StasisEnterState,
            StasisExitState,
            StasisSwitchState);

        stateMachine.AddNewState(
            (int)ProtagState.Jump,
            JumpUpdate,
            JumpFixedUpdate,
            JumpEnterState,
            JumpExitState,
            JumpSwitchState);
    }
    
    #endregion
    
    // State implementations here SINGLE FILE MONOLITH LETS FUCKING GOOOOOO

    #region Idle State

    public void IdleUpdate()
    {
        
    }

    public void IdleFixedUpdate()
    {
        float timeStep = Time.fixedDeltaTime;
        protagMover.GroundedFriction(timeStep);
        rotationResolver.GroundedRotationResolve(timeStep);
    }

    public void IdleEnterState()
    {
        inputProvider.OnJumpPressed += EnterJump;
    }

    public void IdleExitState()
    {
        inputProvider.OnJumpPressed -= EnterJump;
    }

    public int IdleSwitchState()
    {
        if (!physicsState.IsGrounded)
        {
            return (int)ProtagState.Airborne;
        }
        if (inputProvider.HorizontalMovementNormalized != Vector2.zero)
        {
            return (int)ProtagState.Walk;
        }

        return -1;
    }
    
    #endregion

    #region Walk State
    public void WalkUpdate()
    {
        
    }

    public void WalkFixedUpdate()
    {
        Vector2 input = protagFPCam.TransformInput(inputProvider.HorizontalMovementNormalized);
        float timeStep = Time.fixedDeltaTime;
        protagMover.GroundedMoveTowards(input, timeStep);
        protagMover.GroundedSnap();
        rotationResolver.GroundedRotationResolve(timeStep);
    }

    public void WalkEnterState()
    {
        inputProvider.OnJumpPressed += EnterJump;
    }

    public void WalkExitState()
    {
        inputProvider.OnJumpPressed -= EnterJump;
    }

    public int WalkSwitchState()
    {
        if (!physicsState.IsGrounded)
        {
            return (int)ProtagState.Airborne;
        }

        if (inputProvider.HorizontalMovementNormalized == Vector2.zero)
        {
            return (int)ProtagState.Idle;
        }
        
        return -1;
    }
    
    #endregion

    #region Jump State

    private float jumpStateTime;

    public void EnterJump()
    {
        stateMachine.SwitchState((int)ProtagState.Jump);
    }

    public void JumpUpdate()
    {

    }

    public void JumpFixedUpdate()
    {
        float timeStep = Time.fixedDeltaTime;
        rotationResolver.AirborneRotationResolve(timeStep);
    }

    public void JumpEnterState()
    {
        jumpStateTime = Time.time;
        protagMover.Jump();
    }

    public void JumpExitState()
    {
        
    }

    public int JumpSwitchState()
    {
        // Prevent instantly being grounded when jumping
        if (Time.time - jumpStateTime < 0.2f) 
            return -1;
        
        if (physicsState.IsGrounded)
        {
            return (int)ProtagState.Idle;
        }
        else
        {
            return (int)ProtagState.Airborne;
        }
    }

    #endregion
    
    #region Airborne State

    public void AirborneUpdate()
    {

    }

    public void AirborneFixedUpdate()
    {
        float timeStep = Time.fixedDeltaTime;
        rotationResolver.AirborneRotationResolve(timeStep);
    }

    public void AirborneEnterState()
    {

    }

    public void AirborneExitState()
    {

    }

    public int AirborneSwitchState()
    {
        if (physicsState.IsGrounded)
        {
            return (int)ProtagState.Idle;
        }
        
        return -1;
    }

    #endregion

    #region Jetpack State

    public void JetpackUpdate()
    {

    }

    public void JetpackFixedUpdate()
    {

    }

    public void JetpackEnterState()
    {

    }

    public void JetpackExitState()
    {

    }

    public int JetpackSwitchState()
    {
        return -1;
    }

    #endregion

    #region Stasis State

    public void StasisUpdate()
    {
        
    }

    public void StasisFixedUpdate()
    {

    }

    public void StasisEnterState()
    {

    }

    public void StasisExitState()
    {

    }

    public int StasisSwitchState()
    {
        return -1;
    }

    #endregion
}
