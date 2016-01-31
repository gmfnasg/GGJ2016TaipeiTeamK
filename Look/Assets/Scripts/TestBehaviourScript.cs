using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

public class TestBehaviourScript : MonoBehaviour {

    WorldCondition worldCond = new WorldCondition();
    ConditionTable condTable = new ConditionTable();

	public List<String> Objlist = new List<String>();
	public List<String> Targetlist = new List<String>();

	// Use this for initialization
	void Start () {
        System.Random rand = new System.Random();

        worldCond.generate(rand);
        condTable.generate( rand , worldCond , 6 , 3 ,  3 );

        for (int i = 0; i < condTable.numSelection; ++i)
        {
            Condition cond = condTable.getCondition(i);
			Debug.Log(i);
            for (int j = 0; j < cond.getExprissionNum(); ++j)
            {
				
                String str = cond.getContent(j);

				Objlist.Add(str);

            }
			Targetlist.Add(cond.getTarget());
        }
		fnCreateFile();
	}
	void fnCreateFile()
	{

		StreamWriter t_sStreamWriter; // 文件流信息
		FileInfo t_fFileInfo = new FileInfo(Application.dataPath + "//" + "儀式的條件.txt");
		if (!t_fFileInfo.Exists)
		{
			t_sStreamWriter = t_fFileInfo.CreateText();

			// 如果此文件不存在则创建
		}
		else
		{
			t_sStreamWriter = t_fFileInfo.AppendText(); // 如果此文件存在则打开

		}
		for(int i=0 ; i < 6 ; i++)
		{
			t_sStreamWriter.Write("第");
			t_sStreamWriter.Write((i+1).ToString());
			t_sStreamWriter.WriteLine("組");
			t_sStreamWriter.WriteLine(@"------------------------------------------------------");
			for(int j = 0 ; j < 3 ; j++)
			{
				t_sStreamWriter.WriteLine(Objlist[j+i*3]);

			}
			t_sStreamWriter.Write("拿取");
			t_sStreamWriter.WriteLine(Targetlist[i]);
			t_sStreamWriter.WriteLine(@"------------------------------------------------------");
		}
		// 以行的形式写入信息 
		t_sStreamWriter.Close(); //关闭流
		t_sStreamWriter.Dispose(); // 销毁流
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
