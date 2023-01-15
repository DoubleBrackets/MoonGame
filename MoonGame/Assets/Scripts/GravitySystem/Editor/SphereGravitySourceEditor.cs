using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SphereGravitySource)), CanEditMultipleObjects]
public class SphereGravitySourceEditor : Editor
{
    private SerializedObject so;
    private SerializedProperty gravityCollider;
    private SerializedProperty innerRadius;
    
    private void OnEnable()
    {
        so = new SerializedObject(target);
        gravityCollider = so.FindProperty("orbitCollider");
        innerRadius = so.FindProperty("innerDistanceRadius");
    }

    private void OnSceneGUI()
    {
        SphereCollider coll = (SphereCollider)gravityCollider.objectReferenceValue;
        Undo.RecordObject(coll, "Change Radius");
        Handles.color = Color.yellow;
        coll.radius = Handles.RadiusHandle(Quaternion.identity, coll.WorldPos(), coll.radius);
        
        Handles.color = new Color(1f, 0.63f, 0.2f);
        innerRadius.floatValue = Handles.RadiusHandle(Quaternion.identity, coll.WorldPos(), innerRadius.floatValue);
        so.ApplyModifiedProperties();
    }
}
