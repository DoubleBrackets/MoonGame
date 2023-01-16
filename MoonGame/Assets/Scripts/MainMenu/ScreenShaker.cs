using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    [SerializeField] private RecomposeEffectEventChannelSO askStartRecomposeEffect;
    [SerializeField] private float loopTime;

    private float timer = 1f;
    
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= loopTime)
        {
            timer = 0f;
            askStartRecomposeEffect.Raise(CameraRecomposeManager.RecomposeEffect.JetpackShake);
        }
    }
}
