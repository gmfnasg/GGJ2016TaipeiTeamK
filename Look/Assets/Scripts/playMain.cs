﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class playMain : MonoBehaviour
{    
	public bool TimeB = false;
	public bool ChTimeB = false;
	public float TimeTotal;
	public float ChTimeTotal;
	public float ChTimeTotalS;
	public GameObject TimeText;
	public GameObject ChTimeText;
	public bool SpeakingBool;
	public GameObject SpeakingBoolP;
	public float spDamge;
	public GameObject SPP;
	public float spTotal;

    public GameObject button;
    public Animator transition;

    WorldCondition worldCondition = new WorldCondition();

    RoomGenerator roomGenerator;


    private AudioPlayer mAudioPlayer;
    private AudioPlayer audioPlayer
    {
        get
        {
            if (!mAudioPlayer) mAudioPlayer = GameObject.Find("Audio Player").GetComponent<AudioPlayer>();
            return mAudioPlayer;
        }
    }

    void generateWorld(int seed)
    {
        System.Random rand = new System.Random(4563);
        worldCondition.generate(rand);
        roomGenerator.spawnScene(worldCondition);


    }
	void Start ()
    {
        roomGenerator = GetComponent<RoomGenerator>();
        generateWorld(0);

		ChTimeTotalS =ChTimeTotal;
		SpeakingBool =  Random.Range(0.0f,1.0f)>=0.5f ?  true :  false;
		if(SpeakingBool)
		{
			SpeakingBoolP.transform.Rotate(new Vector3(0.0f,180.0f,0.0f));
		}
		if(!SpeakingBool)
		{
			SpeakingBoolP.transform.Rotate(new Vector3(0.0f,0.0f,0.0f));
		}
	}
	
	void Update ()
    {
		if(TimeB)
		{
			TimeCountdow();
			spTotal = SPP.GetComponent<Slider>().value * TimeTotal;
			spTotal -= spDamge * Time.deltaTime;
			SPP.GetComponent<Slider>().value = spTotal/TimeTotal;
		}

		if(ChTimeB)
		{
			ChTimeCountdow();
		}

        if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            openEyes(button);
            TimeBool();
        }
	}

	public void openEyes(GameObject bt)
	{
		//刪掉開眼BT
		Destroy(bt);
        transition.SetTrigger("Fade In");
    }

	public void TimeBool()
	{
		//開眼時觸發遊戲時間流動

		TimeB = true;
		ChTimeB = true;
	}
    
	//顯示倒數計時的文字
	void TimeCountdow(){
		TimeText.GetComponent<Text>().text = TimeTotal.ToString("0") + "秒";
		TimeTotal -= Time.deltaTime;
	}
    void ChTimeCountdow()
    {
        ChTimeText.GetComponent<Text>().text = ChTimeTotal.ToString("0") + " 秒";
        ChTimeTotal -= Time.deltaTime;

        if (ChTimeTotal <= 0f)
        {
            audioPlayer.PlayAudio("Switch Identity");

            ChTimeTotal = ChTimeTotalS;
            SpeakingBool = SpeakingBool == true ? false : true;

            if (SpeakingBool)
            {
                SpeakingBoolP.transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            }
            else
            {
                SpeakingBoolP.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
    }
}
