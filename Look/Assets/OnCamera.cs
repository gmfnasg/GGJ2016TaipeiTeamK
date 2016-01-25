using UnityEngine;
using System.Collections;

public class OnCamera : MonoBehaviour {

    #region 檢查是否在指定攝影機內
    public static bool InCamera(Camera LookCamera, GameObject go, float VisionDistance)
    {
        #region 檢查是否在指定攝影範圍內
        if (LookCamera == null)
            return false;
        Collider collider = go.GetComponent<Collider>();
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
        Vector3 screenPoint = LookCamera.WorldToScreenPoint(go.transform.position);
        Ray ray = LookCamera.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, VisionDistance))
            return false;
        #endregion 取得看到的物體

        //檢查看到的物件是否是自己
        bool result = hit.transform == go.transform;
        #endregion 檢查是否有遮蔽物


        return result;
    }
    #endregion 檢查是否在指定攝影機內
}
