using UnityEngine;
using System.Collections;

public class FindPlayer : MonoBehaviour
{
    string SystemName = "FindPlayer";
    public float VisionDistance = 10;
    public float VisionAngel = 10;
    public bool OnLook = false;

    public float LookDistance = 0;
    public float LookAngel = 0;
    public Vector3 LookVector = Vector3.zero;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckOnLook();
    }

    void CheckOnLook()
    {
        //視線尋找法
        if (PlayerControl.Instance == null)
            return;
        Vector3 playerPos = PlayerControl.Instance.gameObject.transform.position;

        #region 檢查距離
        LookDistance = Vector3.Distance(transform.position, playerPos);
        if (LookDistance > VisionDistance)
        {
            if (OnLook)
                OnDisappear();
            OnLook = false;
            return;
        }
        #endregion 檢查距離

        #region 檢查障礙物
        //取得中新點到玩家之間的向量
        Vector3 pos = transform.position;
        LookVector = new Vector3(playerPos.x - pos.x, playerPos.y - pos.y, playerPos.z - pos.z);

        bool lookPlayer = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, LookVector, out hit, VisionDistance))
        {
            lookPlayer = hit.collider.gameObject == PlayerControl.Instance.gameObject;
        }
        else
        {
            if (OnLook)
                OnDisappear();
            OnLook = false;
        }
        #endregion 檢查障礙物

        #region 檢查角度
        if (lookPlayer)
        {
            LookAngel = Vector3.Angle(transform.forward, LookVector);
            if (LookAngel < VisionAngel)
            {
                if (!OnLook)
                    OnFind();
                OnLook = true;
            }
            else
            {
                if (OnLook)
                    OnDisappear();
                OnLook = false;
            }
        }
        else
        {
            if (OnLook)
                OnDisappear();
            OnLook = false;
        }
        #endregion 檢察角度
    }

    public void OnFind()
    {
        DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
            SystemName,
            this.gameObject.name + "發現主角"));
    }

    public void OnDisappear()
    {
        DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
           DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
           SystemName,
          "主角離開" + this.gameObject.name + "視線"));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward);



        if (LookDistance > VisionDistance)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.blue;
        if (OnLook)
            Gizmos.color = Color.yellow;

        if (LookAngel < VisionAngel)
            Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, LookVector);
    }
}