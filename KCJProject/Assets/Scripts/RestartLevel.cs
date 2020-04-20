using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public AsyncOperation async;
    private GameManager code;

    private void Start()
    {
        code = GameManager.instance;
    }

    public void Restart()
    {
        code.isDeath = false;
        Scene currScene = SceneManager.GetActiveScene();
        async = SceneManager.LoadSceneAsync(currScene.buildIndex - 1);
        async.allowSceneActivation = true;
    }
}
