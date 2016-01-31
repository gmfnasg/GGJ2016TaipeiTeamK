using UnityEngine;
using System.IO;
using System;
using System.Collections;
public class CGraetText : MonoBehaviour 
{
	private string m_sFileName = "FileName.txt"; // 文件名
	//private string m_sPath = Application.persistentDataPath; // 路径
	private ArrayList m_aArray; // 文本中每行的内容
	public GameObject Test;
	TestBehaviourScript tbs;
	/*
     * sPath：文件创建目录
     * sName：文件的名称
     * nDate：数据
     */
	void Start(){
		tbs = Test.GetComponent<TestBehaviourScript>();


	
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
				t_sStreamWriter.WriteLine(tbs.Objlist[j+i*3]);

			}
			t_sStreamWriter.Write("拿取");
			t_sStreamWriter.WriteLine(tbs.Targetlist[i]);
			t_sStreamWriter.WriteLine(@"------------------------------------------------------");
		}
		 // 以行的形式写入信息 
		t_sStreamWriter.Close(); //关闭流
		t_sStreamWriter.Dispose(); // 销毁流
	}
	/*
     * path：读取文件的路径
     * name：读取文件的名称
     */
	ArrayList fnLoadFile(string sPath, string sName)
	{
		StreamReader t_sStreamReader = null; // 使用流的形式读取
		//try
		//{
		t_sStreamReader = File.OpenText(sPath + "//" + sName);
		//}
		//catch (Exception ex)
		//{
		//    return null;
		//}
		string t_sLine; // 每行的内容
		ArrayList t_aArrayList = new ArrayList(); // 容器
		while ((t_sLine = t_sStreamReader.ReadLine()) != null)
		{
			t_aArrayList.Add(t_sLine); // 将每一行的内容存入数组链表容器中
		}
		t_sStreamReader.Close(); // 关闭流

		t_sStreamReader.Dispose(); // 销毁流

		return t_aArrayList; // 将数组链表容器返回
	}
	/*
     * sPath：删除文件的路径
     * sName：删除文件的名称
     */
	void fnDeleteFile(string sPath, string sName)
	{
		File.Delete(sPath + "//" + sName);
	}
}