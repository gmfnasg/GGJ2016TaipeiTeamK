using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DrawConeCap))]
public class DrawConeCapInspector : Editor
{
    static DrawConeCap DrawConeCap;

    void OnEnable()
    {
        DrawConeCap = (DrawConeCap)target;
    }

    void OnSceneGUI()
    {
        Handles.color = Color.green;

        Handles.ConeCap(0, DrawConeCap.transform.position - DrawConeCap.transform.forward * (DrawConeCap.VisionDistance / 2),
            DrawConeCap.transform.rotation,
            DrawConeCap.VisionDistance);
    }
}
