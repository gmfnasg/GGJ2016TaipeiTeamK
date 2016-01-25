using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FindPlayer))]
public class FindPlayerInspector : Editor
{
    static FindPlayer FindPlayer;

    void OnEnable()
    {
        FindPlayer = (FindPlayer)target;
    }


    void OnSceneGUI()
    {
        //Handles.color = Color.green;
        //Handles.ConeCap(0, FindPlayer.transform.position - FindPlayer.transform.forward* (FindPlayer.VisionDistance/2),
        //    FindPlayer.transform.rotation, 
        //    FindPlayer.VisionDistance);
    }
}
