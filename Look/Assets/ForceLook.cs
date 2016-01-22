using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class ForceLook : MonoBehaviour {
	string SystemName = "ForceLook";

	public bool CanControl = false; //是否可被操作

    public bool ForceLookCheckDone = false; //注視確認完成

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

    public static Camera Camera3D = null;

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

        if (Camera3D == null)
        {
            GameObject cardboard3dGo = GameObject.Find("CardboardMain 3D");
            if (cardboard3dGo != null)
            {
                Transform mcTransform = cardboard3dGo.transform.FindChild("Head/Main Camera");
                if (mcTransform != null)
                {
                    Camera3D = mcTransform.GetComponent<Camera>();
                }
            }
        }
        if (Camera3D == null)
        {
            Debug.LogError("找不到Camera3D");
        }

        //SetGazedAt(false);

        Color notForceColor = CanControl ? CanControlColor : DontControlColor;
        GetComponent<Renderer>().material.color = notForceColor;

        DebugSystem.AddLogSystem (SystemName);
	}

    void CheckGazedAt()
    {
        if (Camera3D == null)
            return;
        Vector3 pos = Camera3D.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        
        Ray ray = Camera3D.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
            return;

        Color forceColor = CanControl ? CanControOnForcelColor : DontControOnForcelColor;
        Color notForceColor = CanControl ? CanControlColor : DontControlColor;

        if (hit.transform == transform && ForceGo == null)
        {
            CheckDoneTime = Time.time + WatingTime;
            ForceGo = this.gameObject;

            GetComponent<Renderer>().material.color = forceColor;

            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
                DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
                SystemName,
                this.gameObject.name + "被注視"));
        }
        else
        {
            GetComponent<Renderer>().material.color = notForceColor;

            ForceGo = null;
        }
    }
	
	//public void SetGazedAt(bool gazedAt) {
 //       Color forceColor = CanControl ? CanControOnForcelColor : DontControOnForcelColor;
 //       Color notForceColor = CanControl ? CanControlColor : DontControlColor;
	//	GetComponent<Renderer>().material.color = gazedAt ? forceColor : notForceColor;
	//	if (gazedAt && ForceGo == null) {
	//		CheckDoneTime = Time.time + WatingTime;
	//		ForceGo = this.gameObject;
	//		DebugSystem.AddLog (DebugSystem.DebugInfo.GetNewDebugInfo(
	//			DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
	//			SystemName, 
	//			this.gameObject.name + "被注視"));
	//	} else {
	//		ForceGo = null;
	//	}
	//}

	void Update()
	{
        OnLook();

        CheckGazedAt();

        CheckForceLook ();
	}

    //出現在任何一台攝影影機中，包含編輯畫面有出現也會判定為true
    void OnLook()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            if (renderer.isVisible)
            {
                //DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
                //DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
                //SystemName,
                //this.gameObject.name + "被觀看"));
            }
        }
    }

    //出現在任何一台攝影影機的瞬間，包含編輯畫面
    void OnBecameVisible()
    {
        DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
               DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
               SystemName,
               this.gameObject.name + "被發現"));
    }

    //離開在任何一台攝影影機的瞬間，包含編輯畫面
    void OnBecameInvisible()
    {
        DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
               DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
               SystemName,
               this.gameObject.name + "消失了"));
    }

    public void CheckForceLook(){
		if (ForceGo != null && ForceGo == this.gameObject && !ForceLookCheckDone) {
			if (Time.time > CheckDoneTime && !ForceLookCheckDone) {
                ForceLookCheckDone = true;
                DebugSystem.AddLog (DebugSystem.DebugInfo.GetNewDebugInfo(
					DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
					SystemName, 
					"注視"+this.gameObject.name+"確認完成 "));

				if (CanControl && Portal.Instance!=null) {
                    ForceGo = null;
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