using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class ForceLook : MonoBehaviour {
    public float WatingTime = 1;
	public GameObject CardBoard;

    public float CheckDoneTime = -1;
	public GameObject ForceGo = null;

	public static Text DebugInfoWatingTimeText;
	public static Text DebugInfoText;

	// Use this for initialization
	void Start () {
		SetGazedAt(false);
	}
	
	public void SetGazedAt(bool gazedAt) {
		GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
		if (gazedAt && ForceGo != null) {
			CheckDoneTime = Time.time + WatingTime;
			ForceGo = this.gameObject;
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
		}
	}
}