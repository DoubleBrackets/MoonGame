using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Orbiter)), CanEditMultipleObjects]
public class OrbiterEditor : Editor
{
    private SerializedObject so;
    private SerializedProperty radius;
    
    private void OnEnable()
    {
        so = new SerializedObject(target);
        radius = so.FindProperty("radius");
    }

    private void OnSceneGUI()
    {
        Handles.color = Color.green;
        var transform = ((Orbiter)target).transform;
        radius.floatValue = Handles.RadiusHandle(transform.rotation, transform.position, radius.floatValue);
        so.ApplyModifiedProperties();
    }
}
