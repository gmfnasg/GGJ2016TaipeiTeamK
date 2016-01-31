using UnityEngine;
using System.Collections.Generic;
using System;

public class AudioPlayer : MonoBehaviour
{
	public class AudioObject
	{
		public GameObject obj;
        public AudioSource audioSource;
    }

    private List<AudioObject> audioObj = new List<AudioObject>();

    void Awake()
    {
        // 起始時, 先抓取 AudioObjects
        audioObj = getAudioObjects();
    }

    List<AudioObject> getAudioObjects()
    {
        List<AudioObject> tempAudioObjs = new List<AudioObject>();
        
        // 抓取 Audio Player 子物件, 來取得要播放的 Audio Source
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            AudioSource audio = child.GetComponent<AudioSource>();
            if (!audio) continue;  // 如果無 Audio Source 則不抓取此項, 跳至下一項
            
            tempAudioObjs.Add(
                new AudioObject
                {
                    obj = child.gameObject,
                    audioSource = audio
                });
        }
        
        return tempAudioObjs;
    }
		
	// 播放音效
	public void PlayAudio(string audioName)
	{
        AudioObject audioObject = audioObj.Find(audio => audio.obj.name == audioName);
        if (audioObject.audioSource.isActiveAndEnabled) audioObject.audioSource.Play();
	}

    // 抓取音效長度
    public float AudioLength(string audioName)
	{
        AudioObject audioObject = audioObj.Find(audio => audio.obj.name == audioName);
        if (audioObject.audioSource.isActiveAndEnabled)
        {
            AudioSource audioObjSource = audioObject.audioSource;
            return audioObjSource.clip.length / audioObjSource.pitch;
        }
        else return 0.0f;
	}
}
