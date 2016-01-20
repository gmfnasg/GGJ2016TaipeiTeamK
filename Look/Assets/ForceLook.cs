using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class ForceLook : MonoBehaviour {
	string SystemName = "ForceLook";

	public bool CanControl = false; //是否可被操作

	public static float WatingTime = 1;
	public GameObject CardBoard;

    public float CheckDoneTime = -1;
	public static GameObject ForceGo = null;

	public StereoController StereoController3D = null;
	public static float ZoomSpeed = 0.1f;

    public static Color CanControlColor = Color.yellow;
    public static Color CanControOnForcelColor = Color.red;
    public static Color DontControlColor = Color.green;
    public static Color DontControOnForcelColor = Color.blue; 

	// Use this for initialization
	void Start () {
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

		SetGazedAt(false);

		DebugSystem.AddLogSystem (SystemName);
	}
	
	public void SetGazedAt(bool gazedAt) {
        Color forceColor = CanControl ? CanControOnForcelColor : DontControOnForcelColor;
        Color notForceColor = CanControl ? CanControlColor : DontControlColor;
		GetComponent<Renderer>().material.color = gazedAt ? forceColor : notForceColor;
		if (gazedAt && ForceGo == null) {
			CheckDoneTime = Time.time + WatingTime;
			ForceGo = this.gameObject;
			DebugSystem.AddLog (DebugSystem.DebugInfo.GetNewDebugInfo(
				DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
				SystemName, 
				this.gameObject.name + "被注視"));
		} else {
			ForceGo = null;
		}
	}

	void Update()
	{
		CheckForceLook ();
	}

	public void CheckForceLook(){
		if (ForceGo != null && ForceGo == this.gameObject) {
			if (Time.time > CheckDoneTime) {
				ForceGo = null;

				DebugSystem.AddLog (DebugSystem.DebugInfo.GetNewDebugInfo(
					DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
					SystemName, 
					"注視"+this.gameObject.name+"確認完成 "));

				if (CanControl && Portal.Instance!=null) {
					Portal.Instance.DoPortal (CardBoard, this.gameObject);
					if (StereoController3D != null)
						StereoController3D.matchByZoom = 0;
				}
			} else {
				if (StereoController3D != null) {
					float zoom = WatingTime - (CheckDoneTime - Time.time);
					StereoController3D.matchByZoom = zoom;
				}
			}
		}
		if (ForceGo == null && StereoController3D!= null && StereoController3D.matchByZoom>0) {
			StereoController3D.matchByZoom -= ZoomSpeed * Time.deltaTime;
		}
	}
}