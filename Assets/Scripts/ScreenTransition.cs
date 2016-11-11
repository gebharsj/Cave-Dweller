using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ScreenTransition : MonoBehaviour {

    public Image fadePanel;

    public float fadeSpeed;

    public bool fadeIn;
    public bool fadeOut;
    public bool fadeOnStart;

	// Use this for initialization
	void Start () {
        if (fadeOut)
            fadePanel.color = Color.clear;

        if (fadeIn)
            fadePanel.color = Color.black;

        if (fadeOnStart)
            CallCoroutine();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    bool PanelFade()
    {
        if(fadeOut)
        {
            fadePanel.color = Vector4.Lerp(fadePanel.color, Color.black, Time.deltaTime * fadeSpeed);

            if (fadePanel.color == Color.black)
                return true;
        }
        else
        {
            fadePanel.color = Vector4.Lerp(fadePanel.color, Color.clear, Time.deltaTime * fadeSpeed);

            if (fadePanel.color == Color.clear)
                return true;
        }        

        return false;
    }

    IEnumerator HandleFade()
    {
        //yield return new WaitForSeconds(1f);
        yield return new WaitUntil(PanelFade);
    }

    IEnumerator HandleFade(string sceneName)
    {
        //yield return new WaitForSeconds(1f);
        yield return new WaitUntil(PanelFade);
        SceneManager.LoadScene(sceneName);
    }

    public void CallCoroutine()
    {
        StartCoroutine(HandleFade());
    }

    public void CallCoroutine(string sceneName)
    {
        StartCoroutine(HandleFade(sceneName));        
    }
}
