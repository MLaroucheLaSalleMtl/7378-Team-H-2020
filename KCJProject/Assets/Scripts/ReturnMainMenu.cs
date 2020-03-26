using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMainMenu : MonoBehaviour
{
    public AsyncOperation async;

    public void ReturnMenu()
    {
        Scene currScene = SceneManager.GetActiveScene();
        async = SceneManager.LoadSceneAsync(0);
        async.allowSceneActivation = true;
    }
}
