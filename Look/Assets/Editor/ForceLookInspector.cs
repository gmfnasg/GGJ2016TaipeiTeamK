using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor (typeof(ForceLook))]
public class ForceLookInspector : Editor {

	static ForceLook ForceLook;

	void OnEnable () {
		ForceLook = (ForceLook)target ;
	}

	public override void OnInspectorGUI(){
        EditorGUI.BeginChangeCheck();

        if (ForceLook == null)
			return;
		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.LabelField ("資訊 (不可設定)");

        EditorGUILayout.EnumPopup(ForceLook.State);
        GUI.color = Color.yellow;
        EditorGUILayout.Toggle("被注視", ForceLook.OnForceLook);
        EditorGUILayout.Toggle("確定", ForceLook.ForceLookCheckDone);
        GUI.color = Color.white;
        EditorGUILayout.ObjectField("被注視得唯一目標", ForceLook.HaveGameobjectOnForceLook, typeof(GameObject));
        EditorGUILayout.ObjectField("注視攝影機", ForceLook.LookCamera, typeof(Camera));
        EditorGUILayout.ObjectField("推拉近頭控制物件", ForceLook.StereoController3D, typeof(StereoController));
        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical("Box");
        Color checkDoneTimeColor = ForceLook.CheckDoneTime > Time.time ? Color.yellow : Color.white;
        GUI.color = checkDoneTimeColor;
        EditorGUILayout.TextField ("CheckDoneTime: " + ForceLook.CheckDoneTime);
        float leftTime = Time.time > ForceLook.CheckDoneTime ? 0 : ForceLook.CheckDoneTime-Time.time;
        EditorGUILayout.TextField("CheckDone LeftTime: " + ForceLook.CheckDoneTime);
        GUI.color = Color.white;
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical ();

		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.LabelField ("基本設定");
		ForceLook.CanControl = EditorGUILayout.Toggle ("是否可被控制視野",ForceLook.CanControl);
		ForceLook.WatingTime = EditorGUILayout.FloatField ("確認等待時間", ForceLook.WatingTime);
		ForceLook.CardBoard = EditorGUILayout.ObjectField ("CardBoard", ForceLook.CardBoard, typeof(GameObject)) as GameObject; 
		ForceLook.ZoomSpeed = EditorGUILayout.FloatField ("ZoomSpeed", ForceLook.ZoomSpeed);
        ForceLook.VisionDistance = EditorGUILayout.FloatField("可視距離", ForceLook.VisionDistance);
        EditorGUILayout.EndVertical ();

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(ForceLook);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        DrawDefaultInspector();
	}

}
