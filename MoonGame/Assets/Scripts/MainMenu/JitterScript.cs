using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class JitterScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float range;
    private Vector3 pos;

    private void Awake()
    {
        pos = target.position;
    }

    private void Update()
    {
        target.position = pos + new Vector3(
            Random.Range(-range, range),
            Random.Range(-range, range),
            Random.Range(-range, range));
    }
}
