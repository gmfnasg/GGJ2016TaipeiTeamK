using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
	public static Portal Instance;
	string SystemName = "Portal";

	void Awake(){
		Instance = this;
	}

	public void DoPortal(GameObject passengerGo, GameObject targetGo)
	{
		if (passengerGo == null || targetGo == null)
			return;
		passengerGo.transform.position = targetGo.transform.position;

		DebugSystem.AddLog (DebugSystem.DebugInfo.GetNewDebugInfo(
			DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
			SystemName, 
			passengerGo.name + "傳送到物件"+targetGo.name+"的座標 "));
	}

}
