using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{

    [SerializeField] private GameObject Prefab_Wall;

    public List<int> Parameter_List;

    private List<GameObject> Wall;

    private GameObject Wall_1;
    private GameObject Wall_2;
    private GameObject Wall_3;    
    private GameObject Wall_4;

    public Color[] colorSet;

    // Use this for initialization
    void Start()
    {
        //Wall List - 4 Wall;
        List<GameObject> Wall = new List<GameObject>(4);  

        colorSet = new Color[6];

        //From server?
        colorSet[0] = Color.red;
        colorSet[1] = Color.blue;
        colorSet[2] = Color.green;
        colorSet[3] = Color.yellow;
        colorSet[4] = Color.black;
        colorSet[5] = Color.cyan;

        //generate parameters for setting Wall colors
        Parameter_List = GenerateNumber(4, 6);

        //To creat 4 Wall;
        Wall.Add(Instantiate(Prefab_Wall) as GameObject);
        Wall.Add(Instantiate(Prefab_Wall) as GameObject);
        Wall.Add(Instantiate(Prefab_Wall) as GameObject);
        Wall.Add(Instantiate(Prefab_Wall) as GameObject);


        Wall[0].transform.position = new Vector3(0, 25, 25);
        Wall[0].transform.Rotate(new Vector3(0, 90, 0));
        Wall[0].GetComponent<MeshRenderer>().material.color = colorSet[Parameter_List[0]];


        Wall[1].transform.position = new Vector3(-25, 25, 0);
        Wall[1].GetComponent<MeshRenderer>().material.color = colorSet[Parameter_List[1]];


        Wall[2].transform.position = new Vector3(0, 25, -25);
        Wall[2].transform.Rotate(new Vector3(0, 90, 0));
        Wall[2].GetComponent<MeshRenderer>().material.color = colorSet[Parameter_List[2]];


        Wall[3].transform.position = new Vector3(25, 25, 0);
        Wall[3].GetComponent<MeshRenderer>().material.color = colorSet[Parameter_List[3]];

    }
	
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            Application.LoadLevel("End_Scene");
        }
    }

    public List<int> GenerateNumber(int List_size, int Max)  
    {  
        List<int> result = new List<int>(List_size);  
        int temp = 0;

        while (result.Count < List_size)  
        {  
            temp = (int)Random.Range(0, Max);

            if (!result.Contains(temp))  
            {  
                result.Add(temp);  
            }  
        }  
        return result;  
    }
}
