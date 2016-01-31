using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Opening : MonoBehaviour
{
    private Animator mTransition;
    private Animator transition
    {
        get
        {
            if (!mTransition) mTransition = GameObject.Find("Transition").GetComponent<Animator>();
            return mTransition;
        }
    }

    private AudioManager mAudioManager;
    private AudioManager audioManager
    {
        get
        {
            if (!mAudioManager) mAudioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
            return mAudioManager;
        }
    }

    private AudioPlayer mAudioPlayer;
    private AudioPlayer audioPlayer
    {
        get
        {
            if (!mAudioPlayer) mAudioPlayer = GameObject.Find("Audio Player").GetComponent<AudioPlayer>();
            return mAudioPlayer;
        }
    }

    public string sceneName;

    void Awake()
    {
        transition.Play("Fade In");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Mouse0)) DoStart();
        if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Mouse1)) DoExit();
    }

    public void DoStart()
    {
        audioPlayer.PlayAudio("Button Click");
        StartCoroutine(StartLoadingScene());
    }

    IEnumerator StartLoadingScene()
    {
        transition.Play("Fade Out");
        audioManager.audioState = "Fade Out";

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(sceneName);
    }

    public void DoExit()
    {
        audioPlayer.PlayAudio("Button Click");
        StartCoroutine(WaitForAudios("Button Click"));
    }

    IEnumerator WaitForAudios(string audioName)
    {
        float audioLength = audioPlayer.AudioLength(audioName);
        yield return new WaitForSeconds(audioLength);
        Exit();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
