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

    public string sceneName;

    public void DoStart()
    {
        StartCoroutine(StartLoadingScene());
    }

    IEnumerator StartLoadingScene()
    {
        transition.Play("Fade Out");

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
}
