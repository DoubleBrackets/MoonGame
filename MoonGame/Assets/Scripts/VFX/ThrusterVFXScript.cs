using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterVFXScript : MonoBehaviour
{
    [ColorHeader("Listening - Ask Enable/Disable Thruster Channel", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private VoidEventChannelSO askEnableThrusters;
    [SerializeField] private VoidEventChannelSO askDisableThrusters;

    [ColorHeader("Wall Avoidance")] 
    [SerializeField] private LayerMask wallDetection;
    [SerializeField, RangeSlider(0, 3f)] private Vector2 detectionRange;
    [SerializeField, Range(0, 4f)] private float moveBackDistance;
    private float targetDist;
    
    [ColorHeader("Config", ColorHeaderColor.Config)]
    [SerializeField] private float transitionDuration;

    [ColorHeader("Dependencies Thruster Materials")] 
    [SerializeField] private List<Material> thrusterMats;
    
    private Coroutine currentTransition;

    private int enabledParamID;

    private void OnEnable()
    {
        enabledParamID = Shader.PropertyToID("_Enabled");
        askEnableThrusters.OnRaised += EnableThrusters;
        askDisableThrusters.OnRaised += DisableThrusters;
        SetEnabledParamForAll(0);
    }

    private void OnDisable()
    {
        askEnableThrusters.OnRaised -= EnableThrusters;
        askDisableThrusters.OnRaised -= DisableThrusters;
    }

    private void Update()
    {
        targetDist = 0f;
        if (Physics.SphereCast(transform.position, 0.4f,transform.forward, out RaycastHit info, detectionRange.y, wallDetection))
        {
            float t = Mathf.InverseLerp(detectionRange.y, detectionRange.x, info.distance);
            targetDist = -moveBackDistance * t;
        }

        var cPos = transform.localPosition;
        cPos.z = Mathf.Lerp(cPos.z, targetDist, 20f * Time.deltaTime);
        transform.localPosition = cPos;
    }

    private void EnableThrusters()
    {
        StopCurrentTransition();
        StartCoroutine(CoroutTurnOnThrusters());
    }

    private IEnumerator CoroutTurnOnThrusters()
    {
        float time = 0f;
        while (time < transitionDuration)
        {
            time += Time.deltaTime;
            SetEnabledParamForAll(Mathf.SmoothStep(0f, 1f, time / transitionDuration));
            yield return new WaitForEndOfFrame();
        }

        SetEnabledParamForAll(1f);
    }

    private void DisableThrusters()
    {
        StopCurrentTransition();
        StartCoroutine(CoroutTurnOffThrusters());
    }

    private IEnumerator CoroutTurnOffThrusters()
    {
        float time = 0f;
        while (time < transitionDuration)
        {
            time += Time.deltaTime;
            SetEnabledParamForAll(Mathf.SmoothStep(0f, 1f, 1 - time / transitionDuration));
            yield return new WaitForEndOfFrame();
        }

        SetEnabledParamForAll(0f);
    }
    
    private void SetEnabledParamForAll(float val)
    {
        foreach (var mat in thrusterMats)
        {
            mat.SetFloat(enabledParamID, val);
        }
    }

    private void StopCurrentTransition()
    {
        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
            currentTransition = null;
        }
    }
}
