using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamAnchorAssign : MonoBehaviour
{
    [ColorHeader("Dependencies")] [SerializeField]
    private GameAnchorsSO gameAnchor;

    private void OnEnable()
    {
        gameAnchor.mainCam = this.GetComponent<Camera>();
    }
}
