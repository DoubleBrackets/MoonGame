using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCameraController : MonoBehaviour
{
    [ColorHeader("Dependencies")] 
    [SerializeField] private Transform targetCam;
    [SerializeField] private Transform transformAnchor;
    [SerializeField] private InputProvider inputProvider;
    [SerializeField] private FPCameraProfileSO profile;

    // Internal state
    [SerializeField, ReadOnly] private float xRot;
    [SerializeField, ReadOnly] private float xTargetRot;
    [SerializeField, ReadOnly] private float yRot;
    [SerializeField, ReadOnly] private float yTargetRot;

    private Matrix4x4 camLocalRotMtx;
    private Matrix4x4 finalRotMtx;

    public Vector3 Forward => targetCam.forward;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UpdateCamera();
    }

    private void Update()
    {
        UpdateCamera();
    }

    public void UpdateCamera()
    {
        // Calculate axis rotation based on mouse movement
        yTargetRot += inputProvider.MouseDelta.x * profile.horizontalSensitivity;
        xTargetRot += -inputProvider.MouseDelta.y * profile.verticalSensitivity;

        xTargetRot = Mathf.Clamp(xTargetRot, profile.maxDownAngle, profile.maxUpAngle);

        xRot = Mathf.Lerp(xRot, xTargetRot, profile.verticalLerpSpeed * Time.deltaTime);
        yRot = Mathf.Lerp(yRot, yTargetRot, profile.horizontalLerpSpeed * Time.deltaTime);

        // Rotations are relative to global axis, convert to separate quats
        var xRotQuat = Quaternion.Euler(xRot, 0, 0);
        var yRotQuat = Quaternion.Euler(0, yRot, 0);

        // Local rotation is in anchor transform's coords, change basis to world
        camLocalRotMtx = Matrix4x4.Rotate(yRotQuat) * Matrix4x4.Rotate(xRotQuat);

        finalRotMtx = Matrix4x4.Rotate(transformAnchor.rotation) * camLocalRotMtx;

        // Set rotation and position to camera transform
        targetCam.rotation = finalRotMtx.rotation;
        targetCam.position = transformAnchor.position;
    }

    public Vector2 TransformInput(Vector2 input)
    {
        Quaternion rot = Quaternion.Euler(0, 0, -yRot);
        return rot * input;
    }

    private void OnDrawGizmos()
    {
        if(finalRotMtx != default)
            finalRotMtx.DrawMtxGizmo(transformAnchor.position);
    }
}
