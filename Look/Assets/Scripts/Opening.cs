using UnityEngine;
using System.Collections;

public class Opening : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
	
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }

    public void DoStart()
    {
        Application.LoadLevel("GameScene");
    }

    public void DoExit()
    {
        Application.Quit();
    }
}
