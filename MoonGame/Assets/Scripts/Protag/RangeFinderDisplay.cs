using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RangeFinderDisplay : MonoBehaviour
{
    [ColorHeader("Dependencies")]
    [SerializeField] private GameAnchorsSO anchors;
    [SerializeField] private TextMeshProUGUI text;
    
    [ColorHeader("Config", ColorHeaderColor.Config)]
    [SerializeField] private float distanceScale;
    [SerializeField] private float maxDist;
    [SerializeField] private string invalidString;
    [SerializeField] private LayerMask hitMask;
    private void Update()
    {
        var camTransform = anchors.FPCameraTransform;
        Ray rCast = new Ray(camTransform.position, camTransform.forward);
        if (Physics.Raycast(rCast, out RaycastHit hit, maxDist, hitMask))
        {
            text.text = ((int)(hit.distance * distanceScale)).ToString();
        }
        else
        {
            text.text = invalidString;
        }
    }
}
