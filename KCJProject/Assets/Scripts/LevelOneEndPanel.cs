using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelOneEndPanel : MonoBehaviour
{ 
    protected static GameManager code;
    private AsyncOperation async;

    private void Start()
    {
        code = GameManager.instance;
    }

    public void MoveNextLevel()
    {
       //code.isPopUp = false;

       if (async == null)
       {
          Scene currentScene = SceneManager.GetActiveScene();
          async = SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);
          async.allowSceneActivation = true;
       }
    }

    public void ClosePanel()
    {
        code.ActivateController(true);
        code.isPopUp = false;
        code.PauseGame(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        this.gameObject.SetActive(false);
    }
}
