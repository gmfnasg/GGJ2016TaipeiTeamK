using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
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

    public string sceneName;

    public void DoStart()
    {
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
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void DoExit_WaitForAudios(string audioName)
    {
        StartCoroutine(WaitForAudios(audioName));
    }

    IEnumerator WaitForAudios(string audioName)
    {
        float audioLength = GameObject.Find("Audio Player").GetComponent<AudioPlayer>().AudioLength(audioName);
        yield return new WaitForSeconds(audioLength);
        DoExit();
    }
}
