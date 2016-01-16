using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor (typeof(DebugSystem))]
public class DebugSystemInspector : Editor {

	DebugSystem DebugSystem;

	void OnEnable () {
		DebugSystem = (DebugSystem)target ;
	}

	public override void OnInspectorGUI(){

		if (DebugSystem == null)
			return;

		EditorGUI.BeginChangeCheck ();

		#region 參數
		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.LabelField ("參數");

		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.LabelField ("顯示訊息系統列表");
		for (int i = 0; i < DebugSystem.DebugShowSystemLogList.Count; i++) {
			if(DebugSystem.DebugShowSystemLogList[i]==null)
				continue;
			EditorGUILayout.TextField(DebugSystem.DebugShowSystemLogList[i]);
		}
		EditorGUILayout.EndVertical ();

		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.LabelField ("訊息列表");
		for (int i = 0; i < DebugSystem.LogInfoList.Count; i++) {
			if(DebugSystem.LogInfoList[i]==null)
				continue;
			EditorGUILayout.TextArea(DebugSystem.LogInfoList[i].GetLogString());
		}
		EditorGUILayout.EndVertical ();

		EditorGUILayout.EndVertical ();
		#endregion 參數


		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.LabelField ("基本設定");
		DebugSystem.MaxShowLogAmount = EditorGUILayout.IntField ("MaxShowLogAmount: ", DebugSystem.MaxShowLogAmount);
		DebugSystem.DebugInfoText = EditorGUILayout.ObjectField ("DebugInfoText", DebugSystem.DebugInfoText, typeof(Text)) as Text; 
		EditorGUILayout.EndVertical ();

		if (EditorGUI.EndChangeCheck ())
			EditorUtility.SetDirty (DebugSystem);
	}
}
