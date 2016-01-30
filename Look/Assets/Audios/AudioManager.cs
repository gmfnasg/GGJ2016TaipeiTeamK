using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	private string mAudioState = "";
    internal string audioState
    {
        set { mAudioState = value; }
    }

    public AudioMixerSnapshot normal_MusicSnapshot, normal_SfxSnapshot;
	public AudioMixerSnapshot mute_MusicSnapshot, mute_SfxSnapshot;
	public float fadeSpeed = 2.0f;

    public bool fadeInOnAwake;

    void Awake()
    {
        if (fadeInOnAwake)
        {
            normal_MusicSnapshot.TransitionTo(fadeSpeed);
            normal_SfxSnapshot.TransitionTo(fadeSpeed);
        }
    }

	void Update()
	{
		switch(mAudioState)
		{
		case "Fade In":
		{
			normal_MusicSnapshot.TransitionTo(fadeSpeed);
			normal_SfxSnapshot.TransitionTo(fadeSpeed);
			break;
		}
		case "Fade Out":
		{
			mute_MusicSnapshot.TransitionTo(fadeSpeed);
			mute_SfxSnapshot.TransitionTo(fadeSpeed);
			break;
		}
		}
	}
}
