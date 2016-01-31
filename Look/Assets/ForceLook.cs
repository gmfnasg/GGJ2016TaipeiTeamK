using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class ForceLook : MonoBehaviour
{

    public GameObject UI;

    UICreator creator; 

    string SystemName = "ForceLook";

    public bool CanControl = false;
    //是否可被操作
    public bool OnForceLook = false;
    // 是否正被注視
    public bool ForceLookCheckDone = false;
    //注視確認完成

    public float WatingTime = 1;
    public GameObject CardBoard;

    public float CheckDoneTime = -1;
    //記錄注視完成時間

    public StereoController StereoController3D = null;
    //用來控制鏡頭推拉用
    public static float ZoomSpeed = 0.1f;
    //鏡頭推拉速度

    public float VisionDistance = 100;
//可視距離

    //自動轉換攝影機位置
    public static bool AutoChangeControler = false;

    //各種狀態顯示設定
    public static Color CanControlColor = Color.yellow;
    public static Color CanControOnForcelColor = Color.red;
    public static Color DontControlColor = Color.green;
    public static Color DontControOnForcelColor = Color.blue;

    //是否有任何物件被注視
    public static GameObject HaveGameobjectOnForceLook = null;

    public static Camera LookCamera = null;

    //指定傳送點
    public Vector3 PortalTo = Vector3.zero;

    public LookStateEnum LookState = LookStateEnum.Invisible;
    public InteractiveModeEnum InteractiveMode = InteractiveModeEnum.None;

    //觀看狀態
    public enum LookStateEnum
    {
        Invisible,
        //不可視

        OnForceLook,
        //正被注視
        StartForceLook,
        //開始注視
        ExitForceLook,
        //離開注視
        
        OnLook,
        //可被看見
        StartLook,
        //進入視野
        ExitLook,
        //離開視野
    }

    //注視確認後的互動模式
    public enum InteractiveModeEnum
    {
        None,
        //沒有互動
        ControlMe,
        //自己被控制
        ControlOther,
        //別人被控制
        ProtagonistGoNextPoint,
        //主角前進到下一個位置
        ProtagonistStopMove,
        //主角停止移動
        GameOver,
        //遊戲結束
        ResetGame,
        //重新遊玩
    }

    // Use this for initialization
    void Start()
    {
        creator = UI.GetComponent<UICreator>();

        if (StereoController3D == null)
        {
            GameObject cardboard3dGo = GameObject.Find("CardboardMain 3D");
            if (cardboard3dGo != null)
            {
                Transform mcTransform = cardboard3dGo.transform.FindChild("Head/Main Camera");
                if (mcTransform != null)
                {
                    StereoController sc = mcTransform.GetComponent<StereoController>();
                    if (sc != null)
                    {
                        StereoController3D = sc;
                    }
                }
            }
        }
        if (StereoController3D == null)
        {
            Debug.LogError("找不到StereoController3D");
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

        DebugSystem.AddLogSystem(SystemName);
    }

    void Update()
    {
        #region 切換狀態
        ////先轉換前一次的瞬間狀態唯持續狀態
        if (LookState == LookStateEnum.ExitForceLook)
        {
            LookState = LookStateEnum.OnLook;
            OnChangeToOnLook();
        } else if (LookState == LookStateEnum.ExitLook)
        {
            LookState = LookStateEnum.Invisible;
            OnChangeToInvisible();
        } else if (LookState == LookStateEnum.StartLook)
        {
            LookState = LookStateEnum.OnLook;
            OnChangeToOnLook();
        }
        #endregion 切換狀態

        CheckOnLook();

        CheckGazedAt();

        UpdatOnForceLookBehavior();

        #region 正被注視的狀態
        if (LookState == LookStateEnum.OnForceLook)
        {

            switch (InteractiveMode)
            {
                case InteractiveModeEnum.ControlMe:
                    //移動Cardboard到自己的位置
                    if (CanControl && Portal.Instance != null
                        && (AutoChangeControler || (Input.GetKey(KeyCode.Joystick1Button7) && Input.GetKeyDown(KeyCode.Joystick1Button0))))
                    {
                        Portal.Instance.DoPortal(CardBoard, this.gameObject.transform.position);
                        if (StereoController3D != null)
                            StereoController3D.matchByZoom = 0;

                        LookState = LookStateEnum.Invisible;
                        OnChangeToInvisible();
                        HaveGameobjectOnForceLook = null;
                    }
                    break;

                case InteractiveModeEnum.ControlOther:
                    //Cardboard移動到指定的位置
                    if (PortalTo != Vector3.zero)
                    {
                        if (CanControl && Portal.Instance != null
                            && (AutoChangeControler || (Input.GetKey(KeyCode.Joystick1Button7) && Input.GetKeyDown(KeyCode.Joystick1Button0))))
                        {
                            Portal.Instance.DoPortal(CardBoard, PortalTo);
                            if (StereoController3D != null)
                                StereoController3D.matchByZoom = 0;

                            LookState = LookStateEnum.Invisible;
                            OnChangeToInvisible();
                            HaveGameobjectOnForceLook = null;
                        }
                    }
                    break;

                case InteractiveModeEnum.ProtagonistGoNextPoint:
                    // 主角移動到下一個指定位置
                    if (PlayerControl.Instance != null)
                        PlayerControl.Instance.GoNextPoint();
                    break;

                case InteractiveModeEnum.None:
                default:
                    break;
            }
            
        }
        #endregion 正被注視的狀態
    }

    void CheckGazedAt()
    {
        if (LookState == LookStateEnum.Invisible)
            return;
        if (LookState == LookStateEnum.ExitLook)
        {
            LookState = LookStateEnum.Invisible;
            OnChangeToExitLook();
            return;
        }

        #region 取得看到的物體
        if (LookCamera == null)
            return;

        Ray ray = LookCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, VisionDistance))
        {
            if (OnForceLook)
            {
                this.OnForceLook = false;
                this.ForceLookCheckDone = false;

                if (HaveGameobjectOnForceLook == gameObject)
                    HaveGameobjectOnForceLook = null;


                this.LookState = LookStateEnum.ExitForceLook;
                OnChangeToExitForceLook();
            }
            return;
        }
        #endregion 取得看到的物體

        //檢查看到的物件是否是自己
        bool lookMe = hit.transform == transform;

        if (lookMe && OnForceLook)
        {
            LookState = LookStateEnum.OnForceLook;
            OnChangeToOnForceLook();
            return;
        }

        if (lookMe && !OnForceLook)
        {
            this.OnForceLook = true;
            this.CheckDoneTime = Time.time + this.WatingTime;
            this.ForceLookCheckDone = false;

            HaveGameobjectOnForceLook = gameObject;

            this.LookState = LookStateEnum.StartForceLook;
            OnChangeToStartForceLook();
        } else if (!lookMe && OnForceLook)
        {
            this.LookState = LookStateEnum.ExitForceLook;
            this.OnForceLook = false;
            if (HaveGameobjectOnForceLook == gameObject)
                HaveGameobjectOnForceLook = null;
            OnChangeToExitForceLook();

            HaveGameobjectOnForceLook = null;
        } else
        {
            if (this.OnForceLook)
                this.OnForceLook = false;
            if (this.ForceLookCheckDone)
                this.ForceLookCheckDone = false;
            if (this.LookState == LookStateEnum.OnForceLook)
                this.LookState = LookStateEnum.ExitForceLook;
            if (HaveGameobjectOnForceLook == gameObject)
                HaveGameobjectOnForceLook = null;
        }

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


        if (OnCamera.InCamera(LookCamera, gameObject, VisionDistance))
        {
            if (this.LookState == LookStateEnum.Invisible)
            {
                this.LookState = LookStateEnum.StartLook;
                OnChangeToStartLook();
            }
        } else
        {
            if (this.LookState == LookStateEnum.OnLook)
            {
                this.LookState = LookStateEnum.ExitLook;
                OnChangeToExitLook();
            }
        }

    }

    //出現在任何一台攝影影機的瞬間，包含編輯畫面
    void OnBecameVisible()
    {
        if (!OnCamera.InCamera(LookCamera, gameObject, VisionDistance))
            return;
        this.LookState = LookStateEnum.StartLook;
        OnChangeToStartLook();
    }

    //離開在任何一台攝影影機的瞬間，包含編輯畫面
    void OnBecameInvisible()
    {
        this.LookState = LookStateEnum.ExitLook;
        OnChangeToExitLook();
    }

    //更新注視狀態行為
    public void UpdatOnForceLookBehavior()
    {
        if (OnForceLook)
        {
            if (Time.time > CheckDoneTime && !ForceLookCheckDone)
            {
                ForceLookCheckDone = true;
                OnCheckDone();
            } else
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
        } else
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

    #region 改變被注視狀態表現

    void ChangeColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
            return;
        Material material = GetComponent<Renderer>().material;
        if (material == null)
            return;


        if (OnForceLook)
        {
            material.color = CanControl ? CanControOnForcelColor : DontControOnForcelColor;
        } else
        {
            material.color = CanControl ? CanControlColor : DontControlColor;
        }
    }

    #endregion 改變被注視狀態表現

    //#region 檢查是否在指定攝影機內
    //bool OnCamera()
    //{
    //    #region 檢查是否在指定攝影範圍內
    //    if (LookCamera == null)
    //        return false;
    //    Collider collider = GetComponent<Collider>();
    //    if (collider == null)
    //        return false;
    //    Plane[] planes = GeometryUtility.CalculateFrustumPlanes(LookCamera);
    //    bool isOnCamera = GeometryUtility.TestPlanesAABB(planes, collider.bounds);
    //    if (!isOnCamera)
    //        return false;
    //    #endregion 檢查是否在指定攝影範圍內


    //    #region 檢查是否有遮蔽物
    //    //這是簡陋板，只能檢查無體中新點是否有被遮擋無法判定是否有任何地方有顯示
    //    #region 取得看到的物體
    //    if (LookCamera == null)
    //        return false;
    //    Vector3 screenPoint = LookCamera.WorldToScreenPoint(transform.position);
    //    Ray ray = LookCamera.ScreenPointToRay(screenPoint);
    //    RaycastHit hit;
    //    if (!Physics.Raycast(ray, out hit, VisionDistance))
    //        return false;
    //    #endregion 取得看到的物體

    //    //檢查看到的物件是否是自己
    //    bool result = hit.transform == transform;
    //    #endregion 檢查是否有遮蔽物


    //    return result;
    //}
    //#endregion 檢查是否在指定攝影機內

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

    #region 狀態類

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

        creator.OnCancel();
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

    //確認完成
    void OnCheckDone()
    {
        DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "注視" + this.gameObject.name + "確認完成 "));


        if(creator.UIExist == false)
        {
            creator.item_name = this.name;
            creator.UIExist = true;
        }

        #endregion 狀態類
    }
}