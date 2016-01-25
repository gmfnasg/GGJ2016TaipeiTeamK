using UnityEngine;
using System.Collections;

public class FindPlayer : MonoBehaviour
{
    string SystemName = "FindPlayer";
    public Camera Monitor;//監視攝影機
    public float VisionDistance = 10;
    public bool OnLook = false;

    // Use this for initialization
    void Start()
    {
        FintMonitor();
    }

    // Update is called once per frame
    void Update()
    {
        CheckOnLook();
    }

    void CheckOnLook()
    {
        //視線尋找法


        //攝影機尋找法
        //Renderer renderer = gameObject.GetComponent<Renderer>();
        //if (renderer == null)
        //    return;

        //if (!renderer.isVisible)//出現在任何一台攝影影機中，包含編輯畫面有出現也會判定為true
        //    return;

        //GameObject playerGo = PlayerControl.Instance.gameObject;
        //if (playerGo == null)
        //    return;
        
        //if(OnCamera.InCamera(Monitor, gameObject, VisionDistance))
        //{
        //    if (!OnLook)
        //    {
        //        DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
        //            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
        //            SystemName,
        //            this.gameObject.name + "發現主角"));
        //    }

        //    OnLook = true;
        //}
        //else
        //{
        //    if (OnLook)
        //    {
        //        DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
        //            DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
        //            SystemName,
        //            "主角離開"+this.gameObject.name + "視線"));
        //    }

        //    OnLook = false;
        //}
            


        //DebugSystem.AddLog(DebugSystem.DebugInfo.GetNewDebugInfo(
        //DebugSystem.DebugInfo.DebugLogTypeEnum.Info,
        //SystemName,
        //this.gameObject.name + "被觀看"));
    }

    void FintMonitor()
    {
        if (Monitor == null)
        {
            Transform monitorTransform = transform.Find("Monitor");
            if (monitorTransform == null)
                return;
            Monitor = monitorTransform.GetComponent<Camera>();

            if (Monitor == null)
                Debug.LogError("無法找到監視器");
        }
    }
}