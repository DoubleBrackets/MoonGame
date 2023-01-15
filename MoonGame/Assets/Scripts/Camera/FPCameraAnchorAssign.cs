using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCameraAnchorAssign : MonoBehaviour
{
    [ColorHeader("Dependencies")] [SerializeField]
    private GameAnchorsSO gameAnchor;

    private void OnEnable()
    {
        gameAnchor.FPCameraTransform = transform;
    }
}
