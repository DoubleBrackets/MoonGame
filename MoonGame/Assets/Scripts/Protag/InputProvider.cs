using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProvider : MonoBehaviour
{
    private Vector2 horizontalMovementRaw;
    private bool isJumpHeld;
    
    
    public Vector2 HorizontalMovementRaw => horizontalMovementRaw;
    public bool IsJumpHeld => isJumpHeld;
    public Action OnJumpPressed;
    
    void Update()
    {
        horizontalMovementRaw = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));

        isJumpHeld = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpPressed?.Invoke();
        }
    }
}
