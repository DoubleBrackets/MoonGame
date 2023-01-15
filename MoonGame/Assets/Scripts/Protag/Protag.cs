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
    [SerializeField] private RecomposeEffectEventChannelSO askStartCameraRecompose;
    [SerializeField] private VoidEventChannelSO askStartThrusterVFX;
    [SerializeField] private VoidEventChannelSO askEndThrusterVFX;

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

    private void LandingRecomposeEffect()
    {
        askStartCameraRecompose.Raise(CameraRecomposeManager.RecomposeEffect.Landing, Mathf.Clamp(physicsState.VerticalSpeed,6f,8f));
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
        if (Time.time - walkImpulseTimer > 0.75f)
        {
            askStartCameraRecompose.Raise(footstepAlternate ?
                CameraRecomposeManager.RecomposeEffect.Footstep1 :
                CameraRecomposeManager.RecomposeEffect.Footstep2);

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
        walkImpulseTimer = Time.time - 1f;
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
        askStartCameraRecompose.Raise(CameraRecomposeManager.RecomposeEffect.Jump);
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
           LandingRecomposeEffect();
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
            LandingRecomposeEffect();
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
        if (Time.time - shakeTimer >= 0.1f)
        {
            shakeTimer = Time.time;
            askStartCameraRecompose.Raise(CameraRecomposeManager.RecomposeEffect.JetpackShake);
        }
    }

    public void JetpackFixedUpdate()
    {
        float timeStep = Time.fixedDeltaTime;
        protagMovementController.JetpackMovement(protagFPCam.Forward, timeStep);
        if (physicsState.GravityAccelMag > 1.5f)
        {
            rotationResolver.AirborneRotationResolve(timeStep, physicsState.GravityAccelMag);
        }
        else
        {
            //rotationResolver.JetpackRotationResolve(timeStep, protagFPCam.LookRotation);
        }
    }

    public void JetpackEnterState()
    {
        shakeTimer = Time.time;
        askStartThrusterVFX.Raise();
    }

    public void JetpackExitState()
    {
        askEndThrusterVFX.Raise();
    }

    public int JetpackSwitchState()
    {
        if (physicsState.IsGrounded)
        {
            LandingRecomposeEffect();
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
