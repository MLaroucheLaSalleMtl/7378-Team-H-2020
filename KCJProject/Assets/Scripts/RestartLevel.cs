using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public AsyncOperation async;

    public void Restart()
    {
        Scene currScene = SceneManager.GetActiveScene();
        async = SceneManager.LoadSceneAsync(currScene.buildIndex - 1);
        async.allowSceneActivation = true;
    }
}
