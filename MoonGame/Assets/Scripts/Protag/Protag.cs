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
    [SerializeField] private ProtagMovementController protagMovementController;
    [SerializeField] private ProtagRotationResolver rotationResolver;

    [ColorHeader("Invoking - Various Effects Channels", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private ImpulseEffectEventChannelSO askStartCameraImpulse;

    [ColorHeader("Debug Fields")]
    [ReadOnly, SerializeField] private ProtagState currentState;

    // private fields
    private QStateMachine stateMachine;
    
    public Vector2 transformedHInput => protagFPCam.TransformInput(inputProvider.HorizontalMovementNormalized);
    
    private void OnEnable()
    {
        stateMachine = new QStateMachine((int)ProtagState.Idle);
        AddStates();
    }

    private void OnDisable()
    {
        stateMachine.ExitStateMachine();
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
        protagMovementController.GroundedFriction(timeStep);
        rotationResolver.GroundedRotationResolve(timeStep, physicsState.GravityAccelMag);
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

    private float walkImpulseTimer = 0f;
    private bool footstepAlternate = false;
    
    public void WalkUpdate()
    {
        if (Time.time - walkImpulseTimer > 1.35f)
        {
            askStartCameraImpulse.Raise(footstepAlternate ?
                CameraImpulseManager.ImpulseEffect.Footstep1 :
                CameraImpulseManager.ImpulseEffect.Footstep2, 1f);

            footstepAlternate = !footstepAlternate;
            walkImpulseTimer = Time.time;
        }
    }

    public void WalkFixedUpdate()
    {
        Vector2 input = transformedHInput;
        float timeStep = Time.fixedDeltaTime;
        protagMovementController.GroundedMoveTowards(input, timeStep);
        protagMovementController.GroundedSnap();
        rotationResolver.GroundedRotationResolve(timeStep, physicsState.GravityAccelMag);
    }

    public void WalkEnterState()
    {
        walkImpulseTimer = Time.time;
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
        AirborneFixedUpdate();
    }

    public void JumpEnterState()
    {
        jumpStateTime = Time.time;
        protagMovementController.Jump();
    }

    public void JumpExitState()
    {
        
    }

    public int JumpSwitchState()
    {
        // Prevent instantly being grounded when jumping
        if (Time.time - jumpStateTime < 0.25f) 
            return -1;
        
        if (physicsState.IsGrounded)
        {
            askStartCameraImpulse.Raise(CameraImpulseManager.ImpulseEffect.Landing, 1f);
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
        Vector2 input = transformedHInput;
        float timeStep = Time.fixedDeltaTime;
        rotationResolver.AirborneRotationResolve(timeStep, physicsState.GravityAccelMag);
        protagMovementController.AirborneMoveTowards(input, timeStep);
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
            askStartCameraImpulse.Raise(CameraImpulseManager.ImpulseEffect.Landing, 1f);
            return (int)ProtagState.Idle;
        }

        if (inputProvider.IsJumpHeld)
        {
            return (int)ProtagState.Jetpack;
        }
        
        return -1;
    }

    #endregion

    #region Jetpack State

    private float shakeTimer;

    public void JetpackUpdate()
    {
        if (Time.time - shakeTimer >= 0.05f)
        {
            shakeTimer = Time.time;
            askStartCameraImpulse.Raise(CameraImpulseManager.ImpulseEffect.JetpackShake, 1f);
        }
    }

    public void JetpackFixedUpdate()
    {
        protagMovementController.JetpackMovement(protagFPCam.Forward, Time.fixedDeltaTime);
    }

    public void JetpackEnterState()
    {
        shakeTimer = Time.time;
    }

    public void JetpackExitState()
    {

    }

    public int JetpackSwitchState()
    {
        if (physicsState.IsGrounded)
        {
            askStartCameraImpulse.Raise(CameraImpulseManager.ImpulseEffect.Landing, 1f);
            return (int)ProtagState.Idle;
        }
        if (!inputProvider.IsJumpHeld)
        {
            return (int)ProtagState.Airborne;
        }
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
