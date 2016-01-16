using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor (typeof(ForceLook))]
public class ForceLookInspector : Editor {

	ForceLook ForceLook;

	// Use this for initialization
	void OnEnable () {
		ForceLook = (ForceLook)target ;
	}

	public override void OnInspectorGUI(){

		if (ForceLook == null)
			return;
		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.LabelField ("參數");
		if(ForceLook.CheckDoneTime!=null)
			EditorGUILayout.LabelField ("CheckDoneTime: " + ForceLook.CheckDoneTime);
		EditorGUILayout.ObjectField ("ForceGo", ForceLook.ForceGo, typeof(GameObject)); 
		EditorGUILayout.EndVertical ();

		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.LabelField ("基本設定");
		ForceLook.WatingTime = EditorGUILayout.FloatField ("WatingTime", ForceLook.WatingTime);
		ForceLook.CardBoard = EditorGUILayout.ObjectField ("CardBoard", ForceLook.CardBoard, typeof(GameObject)) as GameObject; 
		EditorGUILayout.EndVertical ();

		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.LabelField ("Debug");
		ForceLook.DebugInfoText = EditorGUILayout.ObjectField ("DebugInfoText", ForceLook.DebugInfoText, typeof(Text)) as Text; 
		ForceLook.DebugInfoWatingTimeText = EditorGUILayout.ObjectField ("DebugInfoWatingTimeText", ForceLook.DebugInfoWatingTimeText, typeof(Text)) as Text; 
		EditorGUILayout.EndVertical ();
	}

}
