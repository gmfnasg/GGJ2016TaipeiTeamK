using UnityEngine;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
    public GameObject Plan;
    public Vector3 TargetPos;
    public NavMeshAgent Agent;
    public List<Vector3> Targets;
    public int ToTargetsIndex = 0;
    public Camera LookCamera;


	// Use this for initialization
	void Start () {
        Agent = gameObject.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.N))
        {
            if (Input.GetKeyDown(KeyCode.F1))
                Agent.SetDestination(TargetPos);
            else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                //前進到下個目的地
                if (Targets.Count > 0)
                {
                    if (ToTargetsIndex >= Targets.Count)
                        ToTargetsIndex = 0;
                    Agent.SetDestination(Targets[ToTargetsIndex]);
                    ToTargetsIndex++;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Agent.SetDestination(GetMouseDownPos());
            }
        }
            

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
}
