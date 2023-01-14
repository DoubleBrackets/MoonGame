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
    
    private void OnEnable()
    {
        so = new SerializedObject(target);
        gravityCollider = so.FindProperty("orbitCollider");
    }

    private void OnSceneGUI()
    {
        SphereCollider coll = (SphereCollider)gravityCollider.objectReferenceValue;
        Undo.RecordObject(coll, "Change Radius");
        Handles.color = Color.yellow;
        coll.radius = Handles.RadiusHandle(Quaternion.identity, coll.WorldPos(), coll.radius);
    }
}
