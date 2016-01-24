using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoBoard : MonoBehaviour {
    public string InfomationText;
    public GameObject CardBoard;
    public Vector3 Offset;

    Text InfoText = null;
    GameObject InfoBoardGo = null;

    void Awake()
    {
        CreateInfoBoard();
        GetCardBoard();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CardBoard!=null)
            InfoBoardGo.transform.LookAt(CardBoard.transform.position);
    }

    void CreateInfoBoard()
    {
        if (InfoBoardGo != null)
            return;

        InfoBoardGo = GameObject.Instantiate(Resources.Load("InfoBoard"), transform.position, Quaternion.identity) as GameObject;
        if (InfoBoardGo == null)
        {
            Debug.LogError("無法載入資訊看板InfoBoard prefab");
            return;
        }
        InfoBoardGo.transform.SetParent(transform);
        InfoBoardGo.transform.position = transform.position;
        InfoBoardGo.transform.position += Offset;
        Transform textTransform = InfoBoardGo.transform.Find("TargetCube/InfoText");
        if (textTransform == null)
            return;
        InfoText = textTransform.GetComponent<Text>();
        InfoText.text = InfomationText;
    }

    void GetCardBoard()
    {
        if (CardBoard == null)
        {
            CardBoard = GameObject.Find("Cardboard");
        }
        if (CardBoard == null)
        {
            Debug.LogError("找不到CardBoard");
        }
    }
}
