using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraImpulseManager : MonoBehaviour
{
    public enum ImpulseEffect
    {
        JetpackShake,
        Landing,
        Footstep1,
        Footstep2
    }

    [ColorHeader("Listening - Ask Start/Stop Impulse Effects")]
    [SerializeField] private ImpulseEffectEventChannelSO askStartEffect;

    [ColorHeader("Dependencies")]
    [SerializeField] private CinemachineImpulseSource jetpackShakeSource;
    [SerializeField] private CinemachineImpulseSource footstep1ShakeSource;
    [SerializeField] private CinemachineImpulseSource footstep2ShakeSource;
    [SerializeField] private CinemachineImpulseSource landingShakeSource;

    private void OnEnable()
    {
        askStartEffect.OnRaised += StartEffect;
    }

    private void OnDisable()
    {
        askStartEffect.OnRaised -= StartEffect;
    }

    private void StartEffect(ImpulseEffect effect, float force)
    {
        switch (effect)
        {
            case ImpulseEffect.JetpackShake:
                jetpackShakeSource.GenerateImpulseWithForce(force);
                break;
            case ImpulseEffect.Footstep1:
                footstep1ShakeSource.GenerateImpulseWithForce(force);
                break;
            case ImpulseEffect.Footstep2:
                footstep2ShakeSource.GenerateImpulseWithForce(force);
                break;
            case ImpulseEffect.Landing:
                landingShakeSource.GenerateImpulseWithForce(force);
                break;
        }
    }
}
