using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LoadScreen : MonoBehaviour
{
    private AsyncOperation async;
    [SerializeField] private Image progressbar;
    [SerializeField] private Text txtPercent;
    [SerializeField] private float delay = 0;
    [SerializeField] private int sceneToLoad = -1;
    [SerializeField] private bool waitForUserInput = false;
    private bool ready = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        Input.ResetInputAxes();
        System.GC.Collect();
        Scene currentScene = SceneManager.GetActiveScene();
        if(sceneToLoad == -1)
        {
            async = SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);
        }
        else
        {
            async = SceneManager.LoadSceneAsync(sceneToLoad);
        }
        async.allowSceneActivation = false;
        if(waitForUserInput == false)
        {
            Invoke("Active", delay);
        }
    }
    public void Active()
    {
        ready = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(waitForUserInput && Keyboard.current.aKey.isPressed)
        {
            ready = true;
        }
        if (progressbar)
        {
            progressbar.fillAmount = async.progress + 0.1f;
        }
        if (txtPercent)
        {
            txtPercent.text = ((async.progress + 0.1f) * 100).ToString("f2") + "%";
        }
        if(async.progress > 0.89f && SplashScreen.isFinished && ready)
        {
            async.allowSceneActivation = true;
        }
    }
}
