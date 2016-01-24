using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
	public static Portal Instance;
	string SystemName = "Portal";

	void Awake(){
		Instance = this;
	}

	public void DoPortal(GameObject passengerGo, Vector3 toPos)
	{
		if (passengerGo == null || toPos == null)
			return;
		passengerGo.transform.position = toPos;

		DebugSystem.AddLog (DebugSystem.DebugInfo.GetNewDebugInfo(
			DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
			SystemName, 
			passengerGo.name + "傳送到座標" + toPos));
	}

}
