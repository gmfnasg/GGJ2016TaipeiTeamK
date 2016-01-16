using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class ForceLook : MonoBehaviour {
	string SystemName = "ForceLook";

    public float WatingTime = 1;
	public GameObject CardBoard;

    public float CheckDoneTime = -1;
	public GameObject ForceGo = null;

	// Use this for initialization
	void Start () {
		SetGazedAt(false);

		DebugSystem.AddLogSystem (SystemName);
	}
	
	public void SetGazedAt(bool gazedAt) {
		GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
		if (gazedAt && ForceGo == null) {
			CheckDoneTime = Time.time + WatingTime;
			ForceGo = this.gameObject;
			DebugSystem.AddLog (DebugSystem.DebugInfo.GetNewDebugInfo(
				DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
				SystemName, 
				"開始注視目標"+this.gameObject.name));
		} else {
			ForceGo = null;
		}
	}

	void Update()
	{
		CheckForceLook ();
	}

	public void CheckForceLook(){
		if (ForceGo != null && Time.time>CheckDoneTime) {
			CardBoard.transform.position = this.gameObject.transform.position;
			ForceGo = null;

			DebugSystem.AddLog (DebugSystem.DebugInfo.GetNewDebugInfo(
				DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
				SystemName, 
				"注視完成切換座標"));
		}
	}
}