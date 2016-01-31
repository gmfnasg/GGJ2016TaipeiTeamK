using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{

    public int display_range = 10;
    public WallName []test_name = {WallName.Door, WallName.Bed, WallName.Clock, WallName.Number};
    public Color []colorSet;

    [SerializeField] private GameObject Prefab_Wall_Bed;
    [SerializeField] private GameObject Prefab_Wall_Door;
    [SerializeField] private GameObject Prefab_Wall_Clock;
    [SerializeField] private GameObject Prefab_Wall_Number;
    
 
    [SerializeField] private GameObject Prefab_Ball;
    [SerializeField] private GameObject Prefab_Candle;
    [SerializeField] private GameObject Prefab_Book;
    [SerializeField] private GameObject Prefab_Doll;


    public List<int> Parameter_List;

    public List<int> number_of_item;


    private List<GameObject> Wall;

    // Use this for initialization
    public void spawnScene( WorldCondition worldCond )
    {

        colorSet = new Color[6];
        colorSet[0] = Color.red;
        colorSet[1] = Color.yellow;
        colorSet[2] = Color.green;
        colorSet[3] = Color.blue;
        colorSet[4] = Color.black; 
        colorSet[5] = Color.white;

        //Wall List - 4 Wall;
        List<GameObject> Wall = new List<GameObject>(4);  


        //generate parameters for setting Wall colors
        Parameter_List = GenerateNumber(4, 6);

        number_of_item = GenerateNumber(4, 10);


        for (int i = 0; i < worldCond.objects.Count; ++i  )
        {
            ObjectInfo info = worldCond.objects[i];
            for (int n = 0 ; n < info.num ; ++n )
            {
                switch (info.id)
                {
                    case ObjectId.Ball:
                        Instantiate(Prefab_Ball, new Vector3(Random.Range(-display_range, display_range), 0, Random.Range(-display_range, display_range)), Quaternion.identity);
                        break;
                    case ObjectId.Candlestick:
                        Instantiate(Prefab_Candle, new Vector3(Random.Range(-display_range, display_range), 0, Random.Range(-display_range, display_range)), Quaternion.identity);
                        break;
                    case ObjectId.Book:
                        Instantiate(Prefab_Book, new Vector3(Random.Range(-display_range, display_range), 0, Random.Range(-display_range, display_range)), Quaternion.identity);
                        break;
                    case ObjectId.Doll:
                        Instantiate(Prefab_Doll, new Vector3(Random.Range(-display_range, display_range), 0, Random.Range(-display_range, display_range)), Quaternion.identity);
                        break;
                }
            }
        }


        for (int i = 0; i < 4; ++i )
        {
            WallInfo info = worldCond.wall[i];

            switch(info.name)
            {
            case WallName.Number: Wall.Add(Instantiate(Prefab_Wall_Number) as GameObject); break;
            case WallName.Door: Wall.Add(Instantiate(Prefab_Wall_Door) as GameObject); break;
            case WallName.Clock: Wall.Add(Instantiate(Prefab_Wall_Clock) as GameObject); break;
            case WallName.Bed: Wall.Add(Instantiate(Prefab_Wall_Bed) as GameObject); break;
            }
        }
        //To creat 4 Wall;
        Wall[0].transform.position = new Vector3(0, 25, 25);
        Wall[0].GetComponent<MeshRenderer>().material.color = colorSet[ (int)worldCond.wall[0].color ];

        Wall[1].transform.position = new Vector3(-25, 25, 0);
        Wall[1].transform.Rotate(new Vector3(0, 90, 0));
        Wall[1].GetComponent<MeshRenderer>().material.color = colorSet[(int)worldCond.wall[1].color];


        Wall[2].transform.position = new Vector3(0, 25, -25);
        Wall[2].GetComponent<MeshRenderer>().material.color = colorSet[(int)worldCond.wall[2].color];

        Wall[3].transform.position = new Vector3(25, 25, 0);
        Wall[3].transform.Rotate(new Vector3(0, 90, 0));
        Wall[3].GetComponent<MeshRenderer>().material.color = colorSet[(int)worldCond.wall[3].color];
       
    }
	
    // Update is called once per frame
    void Update()
    {

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
