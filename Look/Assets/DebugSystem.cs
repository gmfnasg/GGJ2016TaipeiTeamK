using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DebugSystem : MonoBehaviour {
	public static int MaxShowLogAmount = 40;
	public static Text DebugInfoText;
	public static List<DebugInfo> LogInfoList = new List<DebugInfo> ();
	public static List<string> DebugShowSystemLogList = new List<string> ();

	public static Text DebugFovText;
	public static StereoController StereoController3D =null;

	public class DebugInfo
	{
		public enum DebugLogTypeEnum
		{
			None = 0,
			Info = 1,
			Warning = 2,
			Error = 3
		}
				
		public List<Color> LogTypeColors = new List<Color>(){
			Color.gray, //None
			Color.white, //Info
			Color.yellow, //Warning 
			Color.red //Error
		};

		public int ID;
		public string Log;
		public DebugLogTypeEnum Type;
		public string SystemType;

		public string GetLogString(){
			return "[<color="+LogTypeColors[(int)Type]+"><<"+Type.ToString()+"(LogID:"+ID+")("+SystemType+")>> </color> "+Log+"]\r\n";
		}

		public static DebugInfo GetNewDebugInfo(DebugLogTypeEnum type, string systemname, string log){
			DebugInfo newDebugInfo = new DebugInfo ();
			newDebugInfo.Type = type;
			newDebugInfo.SystemType = systemname;
			newDebugInfo.Log = log;
			return newDebugInfo;
		}
	}

	// Use this for initialization
	void Start () {
		if (DebugInfoText == null) {
			GameObject debugPanelGo = GameObject.Find ("DebugInfoPanel");
			if (debugPanelGo != null) {
				Transform debugTextTransform = debugPanelGo.transform.FindChild ("Info");
				if (debugTextTransform != null) {
					Text logtext = debugTextTransform.GetComponent<Text> ();
					if (logtext != null) {
						DebugInfoText = logtext;
					}
				}
			} 
		}
		if (DebugInfoText == null)
			Debug.LogError ("無法找到顯示訊息物件");

		if (DebugFovText == null) {
			GameObject debugPanelGo = GameObject.Find ("DebugInfoPanel");
			if (debugPanelGo != null) {
				Transform debugTextTransform = debugPanelGo.transform.FindChild ("Fov");
				if (debugTextTransform != null) {
					Text logtext = debugTextTransform.GetComponent<Text> ();
					if (logtext != null) {
						DebugFovText = logtext;
					}
				}
			} 
		}
		if (DebugFovText == null)
			Debug.LogError ("無法找到顯示鏡頭焦距訊息物件");

		if (StereoController3D == null) {
			GameObject cardboard3dGo = GameObject.Find ("CardboardMain 3D");
			if (cardboard3dGo != null) {
				Transform mcTransform = cardboard3dGo.transform.FindChild ("Head/Main Camera");
				if (mcTransform != null) {
					StereoController sc = mcTransform.GetComponent<StereoController> ();
					if (sc != null) {
						StereoController3D = sc;
					}
				}
			}
		}
		if (StereoController3D == null) {
			Debug.LogError ("找不到StereoController3D");
		}
	}

	public static void AddLog(DebugInfo logInfo){
		if (logInfo == null)
			return;
		if (DebugInfoText != null) {
			logInfo.ID = LogInfoList.Count;
			LogInfoList.Add (logInfo);
			UpdateLogText ();
		}
	}

	static void UpdateLogText(){
		if (DebugInfoText == null)
			return;
		DebugInfoText.text = "";
		int showAmount = 0;
		for (int i = LogInfoList.Count-1; i >0 ; i--) {
			if (!DebugShowSystemLogList.Contains (LogInfoList [i].SystemType))
				continue;

			if (showAmount >= MaxShowLogAmount)
				return;
			
			showAmount++;
			DebugInfoText.text += LogInfoList [i].GetLogString();
		}
	}

	public static void AddLogSystem(string systemName){
		if (string.IsNullOrEmpty (systemName))
			return;
		if (DebugShowSystemLogList.Contains (systemName))
			return;
		DebugShowSystemLogList.Add (systemName);
	}

	void Update(){
		if (DebugFovText != null && StereoController3D != null) {
			DebugFovText.text = "Fov: " + StereoController3D.matchByZoom;
		}
			
	}
}
