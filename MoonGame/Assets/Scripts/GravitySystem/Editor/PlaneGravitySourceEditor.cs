using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlaneGravitySource)), CanEditMultipleObjects]
public class PlaneGravitySourceEditor : Editor
{
    private SerializedObject so;
    private SerializedProperty gravityCollider;
    
    private void OnEnable()
    {
        so = new SerializedObject(target);
        gravityCollider = so.FindProperty("fieldCollider");
    }

    private void OnSceneGUI()
    {
        BoxCollider coll = (BoxCollider)gravityCollider.objectReferenceValue;
        Undo.RecordObject(coll, "Change Height");
        Handles.color = Color.yellow;
        Vector3 pos = coll.center + coll.transform.position;
        var transf = coll.transform;
        var up = transf.up;
        var bounds = coll.bounds;
        Vector3 yBound = up * bounds.extents.y;
        float height = Handles.ScaleSlider(bounds.size.y, pos + yBound, up, transf.rotation, HandleUtility.GetHandleSize(pos - yBound), 0.1f);

        coll.size = new Vector3(coll.size.x, height, coll.size.z);
        coll.center = new Vector3(coll.center.x, height/2f, coll.center.z);
        
    }
}
