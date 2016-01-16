using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor (typeof(ForceLook))]
public class ForceLookInspector : Editor {

	ForceLook ForceLook;

	void OnEnable () {
		ForceLook = (ForceLook)target ;
	}

	public override void OnInspectorGUI(){

		if (ForceLook == null)
			return;
		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.LabelField ("參數");
		EditorGUILayout.LabelField ("CheckDoneTime: " + ForceLook.CheckDoneTime);
		EditorGUILayout.ObjectField ("ForceGo", ForceLook.ForceGo, typeof(GameObject)); 
		EditorGUILayout.EndVertical ();

		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.LabelField ("基本設定");
		ForceLook.WatingTime = EditorGUILayout.FloatField ("WatingTime", ForceLook.WatingTime);
		ForceLook.CardBoard = EditorGUILayout.ObjectField ("CardBoard", ForceLook.CardBoard, typeof(GameObject)) as GameObject; 
		EditorGUILayout.EndVertical ();

	}

}
