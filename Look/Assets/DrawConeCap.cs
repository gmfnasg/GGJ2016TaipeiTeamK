using UnityEngine;
using System.Collections;

public class DrawConeCap : MonoBehaviour {
    public float VisionDistance = 1;

    // Use this for initialization
    void Start () {
        Transform paret = transform.parent;
        if (paret == null)
            return;
        FindPlayer findPlayer = paret.GetComponent<FindPlayer>();
        if (findPlayer == null)
            return;
        VisionDistance = findPlayer.VisionDistance;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
