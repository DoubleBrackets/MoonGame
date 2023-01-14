using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProvider : MonoBehaviour
{
    // Fields
    private Vector2 horizontalMovementNormalized;
    private bool isJumpHeld;
    
    private Vector2 mouseDelta;
    
    // Properties
    public Vector2 HorizontalMovementNormalized => horizontalMovementNormalized;
    public bool IsJumpHeld => isJumpHeld;
    public Vector2 MouseDelta => mouseDelta;
    
    // Events
    public Action OnJumpPressed;

    void Update()
    {
        // Movement
        horizontalMovementNormalized = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")).normalized;

        isJumpHeld = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpPressed?.Invoke();
        }

        // Mouse
        mouseDelta = new Vector2(
            Input.GetAxisRaw("Mouse X"),
            Input.GetAxisRaw("Mouse Y"));
    }
}
