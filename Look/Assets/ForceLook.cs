using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class ForceLook : MonoBehaviour {
	string SystemName = "ForceLook";

	public bool CanControl = false; //是否可被操作
    public bool OnForceLook = false; // 是否正被注視
    public bool ForceLookCheckDone = false; //注視確認完成

	public float WatingTime = 1;
	public GameObject CardBoard;

    public float CheckDoneTime = -1; //記錄注視完成時間

	public StereoController StereoController3D = null; //用來控制鏡頭推拉用
	public static float ZoomSpeed = 0.1f; //鏡頭推拉速度

    public float VisionDistance = 100;//可視距離

    //各種狀態顯示設定
    public static Color CanControlColor = Color.yellow;
    public static Color CanControOnForcelColor = Color.red;
    public static Color DontControlColor = Color.green;
    public static Color DontControOnForcelColor = Color.blue; 

    //是否有任何物件被注視
    public static GameObject HaveGameobjectOnForceLook =null;

    public static Camera LookCamera = null;

    public LookStateEnum State = LookStateEnum.Invisible;

    public enum LookStateEnum
    {
        Invisible,      //不可視

        OnForceLook,    //正被注視
        StartForceLook, //開始注視
        ExitForceLook,  //離開注視
        
        OnLook,         //可被看見
        StartLook,      //進入視野
        ExitLook,       //離開視野
    }

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

        if (LookCamera == null)
        {
            GameObject cardboard3dGo = GameObject.Find("CardboardMain 3D");
            if (cardboard3dGo != null)
            {
                Transform mcTransform = cardboard3dGo.transform.FindChild("Head/Main Camera");
                if (mcTransform != null)
                {
                    LookCamera = mcTransform.GetComponent<Camera>();
                }
            }
        }
        if (LookCamera == null)
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
        if (State == LookStateEnum.Invisible)
            return;
        if (State == LookStateEnum.ExitLook)
        {
            State = LookStateEnum.Invisible;
            OnChangeToExitLook();
            return;
        }

        #region 取得看到的物體
        if (LookCamera == null)
            return;

        Ray ray = LookCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, VisionDistance))
        {
            if (OnForceLook)
            {
                this.OnForceLook = false;
                this.ForceLookCheckDone = false;

                if (HaveGameobjectOnForceLook == gameObject)
                    HaveGameobjectOnForceLook = null;


                this.State = LookStateEnum.ExitForceLook;
                OnChangeToExitForceLook();
            }
            return;
        }
        #endregion 取得看到的物體

        //檢查看到的物件是否是自己
        bool lookMe = hit.transform == transform;

        if (lookMe && OnForceLook)
        {
            State = LookStateEnum.OnForceLook;
            OnChangeToOnForceLook();
            return;
        }

        if (lookMe && !OnForceLook)
        {
            this.OnForceLook = true;
            this.CheckDoneTime = Time.time + this.WatingTime;
            this.ForceLookCheckDone = false;

            HaveGameobjectOnForceLook = gameObject;

            this.State = LookStateEnum.StartForceLook;
            OnChangeToStartForceLook();
        }
        else if(!lookMe && OnForceLook)
        {
            this.State = LookStateEnum.ExitForceLook;
            this.OnForceLook = false;
            if (HaveGameobjectOnForceLook == gameObject)
                HaveGameobjectOnForceLook = null;
            OnChangeToExitForceLook();

            HaveGameobjectOnForceLook = null;
        }
        else
        {
            if(this.OnForceLook)
                this.OnForceLook = false;
            if(this.ForceLookCheckDone)
                this.ForceLookCheckDone = false;
            if(this.State == LookStateEnum.OnForceLook)
                this.State = LookStateEnum.ExitForceLook;
            if(HaveGameobjectOnForceLook==gameObject)
                HaveGameobjectOnForceLook = null;
        }

    }

    void Update()
	{
        #region 切換狀態
        ////先轉換前一次的瞬間狀態唯持續狀態
        if (State == LookStateEnum.ExitForceLook)
        {
            State = LookStateEnum.OnLook;
            OnChangeToOnLook();
        }
        else if (State == LookStateEnum.ExitLook)
        {
            State = LookStateEnum.Invisible;
            OnChangeToInvisible();
        }
        else if (State == LookStateEnum.StartLook)
        {
            State = LookStateEnum.OnLook;
            OnChangeToOnLook();
        }
        #endregion 切換狀態

        CheckOnLook();

        CheckGazedAt();

        UpdatOnForceLookBehavior ();
	}

    
    void CheckOnLook()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer == null)
            return;

        if (!renderer.isVisible)//出現在任何一台攝影影機中，包含編輯畫面有出現也會判定為true
            return;

        //DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
        //DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
        //SystemName,
        //this.gameObject.name + "被觀看"));


        if (OnCamera())
        {
            if (this.State == LookStateEnum.Invisible)
            {
                this.State = LookStateEnum.StartLook;
                OnChangeToStartLook();
            }
        }
        else
        {
            if (this.State == LookStateEnum.OnLook)
            {
                this.State = LookStateEnum.ExitLook;
                OnChangeToExitLook();
            }
        }

    }

    //出現在任何一台攝影影機的瞬間，包含編輯畫面
    void OnBecameVisible()
    {
        if (!OnCamera())
            return;
        this.State = LookStateEnum.StartLook;
        OnChangeToStartLook();
    }

    //離開在任何一台攝影影機的瞬間，包含編輯畫面
    void OnBecameInvisible()
    {
        this.State = LookStateEnum.ExitLook;
        OnChangeToExitLook();
    }

    //更新注視狀態行為
    public void UpdatOnForceLookBehavior(){
        if (OnForceLook)
        {
            if (Time.time > CheckDoneTime && !ForceLookCheckDone)
            {
                ForceLookCheckDone = true;
                DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
                    DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
                    SystemName,
                    "注視" + this.gameObject.name + "確認完成 "));

                if (CanControl && Portal.Instance != null)
                {
                    Portal.Instance.DoPortal(CardBoard, this.gameObject);
                    if (StereoController3D != null)
                        StereoController3D.matchByZoom = 0;

                    State = LookStateEnum.Invisible;
                    OnChangeToInvisible();
                    HaveGameobjectOnForceLook = null;
                }
            }
            else
            {
                if (!ForceLookCheckDone)
                {
                    #region ZOOM IN
                    if (StereoController3D != null)
                    {
                        float zoom = WatingTime - (CheckDoneTime - Time.time);
                        StereoController3D.matchByZoom = zoom;
                    }
                    #endregion ZOOM IN
                }
            }
        }
        else
        {
            #region ZOOM OUT
            if (StereoController3D != null
                && StereoController3D.matchByZoom > 0
                && !ForceLookCheckDone
                && HaveGameobjectOnForceLook == null)
            {
                StereoController3D.matchByZoom -= ZoomSpeed * Time.deltaTime;
            }
            #endregion ZOOM OUT
        }
    }


    //未被看見
    void OnChangeToInvisible()
    {
    }

    //正被注視
    void OnChangeToOnForceLook()
    {
    }

    //開始注視
    void OnChangeToStartForceLook()
    {
        ChangeColor();
        DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "開始注視" + this.gameObject.name));
    }

    //離開注視
    void OnChangeToExitForceLook()
    {
        ChangeColor();
        DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
                DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
                SystemName,
                "離開注視" + this.gameObject.name));
    }

    //可被看見
    void OnChangeToOnLook()
    {
    }

    //進入視野
    void OnChangeToStartLook()
    {
        DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
               DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
               SystemName,
               this.gameObject.name + "被發現"));
    }

    //離開視野
    void OnChangeToExitLook()
    {
        DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
               DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
               SystemName,
               this.gameObject.name + "消失了"));
    }


    void ChangeColor()
    {
        #region 改變被注視狀態表現
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
            return;
        Material material = GetComponent<Renderer>().material;
        if (material == null)
            return;


        if (OnForceLook)
        {
            material.color = CanControl ? CanControOnForcelColor : DontControOnForcelColor;
        }
        else
        {
            material.color = CanControl ? CanControlColor : DontControlColor;
        }
        
        #endregion 改變被注視狀態表現
    }

    #region 檢查是否在指定攝影機內
    bool OnCamera()
    {
        #region 檢查是否在指定攝影範圍內
        if (LookCamera == null)
            return false;
        Collider collider = GetComponent<Collider>();
        if (collider == null)
            return false;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(LookCamera);
        bool isOnCamera = GeometryUtility.TestPlanesAABB(planes, collider.bounds);
        if (!isOnCamera)
            return false;
        #endregion 檢查是否在指定攝影範圍內


        #region 檢查是否有遮蔽物
        //這是簡陋板，只能檢查無體中新點是否有被遮擋無法判定是否有任何地方有顯示
        #region 取得看到的物體
        if (LookCamera == null)
            return false;
        Vector3 screenPoint = LookCamera.WorldToScreenPoint(transform.position);
        Ray ray = LookCamera.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, VisionDistance))
            return false;
        #endregion 取得看到的物體

        //檢查看到的物件是否是自己
        bool result = hit.transform == transform;
        #endregion 檢查是否有遮蔽物


        return result;
    }
    #endregion 檢查是否在指定攝影機內

    #region 檢查是否超過視野距離
    bool CanSee()
    {
        #region 取得看到的物體
        if (LookCamera == null)
            return false;
        Vector3 screenPoint = LookCamera.WorldToScreenPoint(transform.position);
        Ray ray = LookCamera.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, VisionDistance))
            return false;
        #endregion 取得看到的物體

        //檢查看到的物件是否是自己
        bool result = hit.transform == transform;

        return result;
    }
    #endregion
}