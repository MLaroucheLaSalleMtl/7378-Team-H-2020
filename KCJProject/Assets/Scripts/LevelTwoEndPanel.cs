using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTwoEndPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel; // Attach the end of level two panel
    private bool alreadyInsideCollider = false;
    private GameManager code;

    private AsyncOperation async;

    // Start is called before the first frame update
    void Start()
    {
        code = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        if (alreadyInsideCollider == false)
        {
            //code.isPopUp = true;
            //code.PauseGame(true);
            //panel.gameObject.SetActive(true);
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            alreadyInsideCollider = true;
            code.FinishLevel();

            if (async == null)
            {
                Scene currentScene = SceneManager.GetActiveScene();
                async = SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);
                async.allowSceneActivation = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        alreadyInsideCollider = false;
    }

}

