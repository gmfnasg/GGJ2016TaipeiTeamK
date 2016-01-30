using UnityEngine;
using System;

public class AudioPlayer : MonoBehaviour
{
	[Serializable]
	public class AudioObject
	{
		public string name;
		public GameObject obj;
		internal AudioSource audioSource;
	}
	public AudioObject[] audioObject = new AudioObject[]{};

	void Awake()
	{
		for(int i = 0; i < audioObject.Length; i++)
		{
			audioObject[i].audioSource = audioObject[i].obj.GetComponent<AudioSource>();
		}
	}
		
	// 播放音效
	public void PlayAudio(string audioName)
	{
		for(int i = 0; i < audioObject.Length; i++)
		{
			if(audioObject[i].name == audioName && audioObject[i].audioSource.isActiveAndEnabled)
			{
				audioObject[i].audioSource.Play();
			}
		}
	}

    // 抓取音效長度
    public float AudioLength(string audioName)
	{
		for(int i = 0; i < audioObject.Length; i++)
		{
			if(audioObject[i].name == audioName)
			{
				AudioSource audioObjSource = audioObject[i].audioSource;
				return audioObjSource.clip.length / audioObjSource.pitch;
			}
		}
		return 0.0f;
	}
}
