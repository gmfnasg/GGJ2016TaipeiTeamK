using UnityEngine;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
    public string SystemName = "PlayerControl";

    public GameObject Plan;
    public Vector3 TargetPos;
    public NavMeshAgent Agent;
    public List<Vector3> Targets;
    public int ToTargetsIndex = 0;
    public Camera LookCamera;
    public float MoveSpeed = 0.1f;


	// Use this for initialization
	void Start () {
        Agent = gameObject.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.N) || Input.GetKey(KeyCode.Joystick1Button6))
        {
            if (Input.GetKeyDown(KeyCode.F1))
                Agent.SetDestination(TargetPos);
            else if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Joystick1Button5)) {
                //前進到下個目的地
                if (Targets.Count > 0)
                {
                    if (ToTargetsIndex >= Targets.Count)
                        ToTargetsIndex = 0;

                    DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
                    DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
                    SystemName,
                    gameObject.name + "前往(編號"+ ToTargetsIndex + ") "+ Targets[ToTargetsIndex]));

                    Agent.SetDestination(Targets[ToTargetsIndex]);
                    ToTargetsIndex++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                //回到上個目的地
                if (Targets.Count > 0)
                {
                    if (ToTargetsIndex >= Targets.Count)
                        ToTargetsIndex = 0;

                    DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
                    DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
                    SystemName,
                    gameObject.name + "回到(編號" + ToTargetsIndex + ") " + Targets[ToTargetsIndex]));

                    Agent.SetDestination(Targets[ToTargetsIndex]);
                    ToTargetsIndex++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Agent.SetDestination(GetMouseDownPos());
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Joystick1Button8))
            {
                DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
                    DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
                    SystemName,
                    gameObject.name + "停止移動"));

                Agent.Stop();
            }
            else if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Joystick1Button9))
            {
                DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
                    DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
                    SystemName,
                    gameObject.name + "繼續移動"));

                Agent.Resume();
            }
        }

        Move();

        //Debug.Log("玩家自動搜尋路徑狀態" + Agent.pathStatus);   
	}

    #region 取得點到的座標
    Vector3 GetMouseDownPos()
    {
        if (LookCamera == null)
            return Vector3.zero;
        Ray ray = LookCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
            return Vector3.zero;
        if (hit.transform.gameObject != gameObject)
            return Vector3.zero;

        return hit.point;
    }
    #endregion 取得點到的座標

    void Move()
    {
        float horizontal = Input.GetAxisRaw("RightStickHorizontal");
        float Vertical = Input.GetAxis("RightStickVertical");

        //Debug.Log("手把控制 horizontal("+ horizontal + ") Vertical("+ Vertical + ")");

        if ( horizontal== 0 && Vertical == 0)
            return;
        Vector3 newPos = Vector3.zero;
        newPos = newPos + (transform.forward * Vertical * MoveSpeed);
        newPos = newPos + (transform.right * horizontal * MoveSpeed);
        transform.Translate(newPos, Space.World);
    }

    //顯示輸入資訊
    void LogKeyInputInfo()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button0"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button1"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button2"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button3))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button3"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button4"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button5"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button6))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button6"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button7"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button8))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button8"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button9))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button9"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button10))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button10"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button11))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button11"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button12))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button12"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button13))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button13"));
        if (Input.GetKeyDown(KeyCode.Joystick1Button14))
            DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            "Joystick1Button14"));
    }
}
