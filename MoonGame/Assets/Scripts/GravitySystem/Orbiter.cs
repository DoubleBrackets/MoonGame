using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : MonoBehaviour
{
    [ColorHeader("Dependencies")]
    [SerializeField] private Transform targetObject;

    [ColorHeader("Config", ColorHeaderColor.Config)] 
    [SerializeField] private float radius;
    [SerializeField] private float startAngle;
    [SerializeField] private float orbitPeriod;
    [SerializeField] private bool updatePosition;


    private float time;

    private void OnEnable()
    {
        time = startAngle;
    }

    private void OnValidate()
    {
        if (targetObject == null || !updatePosition) return;
        SetPosition(startAngle * Mathf.Deg2Rad);
    }

    private void FixedUpdate()
    {
        if (orbitPeriod == 0)
            return;
        time += Time.fixedDeltaTime;
        float angle = Mathf.PI * 2 * time / orbitPeriod;
        SetPosition(angle);
    }

    private void SetPosition(float angle)
    {
        Vector3 localPos = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
        targetObject.transform.position = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).MultiplyPoint(localPos);
    }
}
